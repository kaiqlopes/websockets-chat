using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebSocketsChatStudy.DTOs;
using WebSocketsChatStudy.Exceptions;
using WebSocketsChatStudy.Models.User;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByNameAsync(loginDTO.Email!);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            throw new InvalidCredentialsException("Incorrect user or password.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

        await _userManager.UpdateAsync(user);

        return new LoginResponseDTO
        {
            UserId = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        };
    }

    public async Task<TokenDTO> RefreshToken(TokenDTO tokenDTO)
    {
        if (tokenDTO == null)
            throw new ArgumentNullException("Invalid client request");

        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDTO.AccessToken!, _configuration);

        if (principal == null)
            throw new InvalidCredentialsException("Invalid access/refresh token");

        var email = principal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new InvalidCredentialsException("Invalid access/refresh token");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims, _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new TokenDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
    }

    public async Task Register(RegisterUserDTO registerUserDTO)
    {
        var userExists = await _userManager.FindByEmailAsync(registerUserDTO.Email!);

        if (userExists != null)
            throw new InvalidOperationException("An account with this email already exists.");

        User user = new()
        {
            Email = registerUserDTO.Email!,
            FirstName = registerUserDTO.FirstName,
            UserName = registerUserDTO.Email!,
            LastName = registerUserDTO.LastName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, registerUserDTO.Password!);

        if (!result.Succeeded)
            throw new InvalidOperationException("User creation failed! Please check user details and try again.");
    }

    public async Task RekoveAccess(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
            throw new ResourceNotFoundException("User not found");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
    }
}
