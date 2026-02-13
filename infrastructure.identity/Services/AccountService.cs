using core.application.DTOs.Users;
using core.application.Enums;
using core.application.Exceptions;
using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Settings;
using infrastructure.identity.Helpers;
using infrastructure.identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace infrastructure.identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtsettings;
        private readonly IDateTimeService _dateTimeService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, JWTSettings jwtsettings, IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtsettings = jwtsettings;
            _dateTimeService = dateTimeService;
        }

        public async Task<Response<AuthenticationResponseDto>> AuthenticateAsync(AuthenticationRequestDto request, string ipAddress)
        {
            //buscar email
            var usuario = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new ApiException($"El email {request.Email} no se encuentra registrado.");

            //loggear
            var result = await _signInManager.PasswordSignInAsync(usuario.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new ApiException($"Las credenciales del usuario no son válidas.");

            //generar token
            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(usuario);

            AuthenticationResponseDto response = new AuthenticationResponseDto();

            response.Id = usuario.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = usuario.Email;
            response.UserName = usuario.UserName;

            var rolesList = await _userManager.GetRolesAsync(usuario).ConfigureAwait(false);
            
            response.Roles = rolesList.ToList();
            response.IsVerified = usuario.EmailConfirmed;

            var refreshToken = GenerateRefreshToken(ipAddress);

            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponseDto>(response, $"Usario Autenticado {usuario.UserName}");
        }

       

        public async Task<Response<string>> RegisterAsync(RegisterRequestDto request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null) throw new ApiException($"El nombre de usuario {request.UserName} ya se encuentra registrado.");

            var usuario = new ApplicationUser
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null) throw new ApiException($"El email {request.Email} ya se encuentra registrado.");

            //crear usuario
            else
            {
                var result = await _userManager.CreateAsync(usuario, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario, Roles.Basic.ToString());
                    return new Response<string>(usuario.Id, message: $"Usuario registrado exitosamente. {request.UserName}");
                }
                else throw new ApiException($"{result.Errors}.");
            }
        }

        //
        // metodos privados
        //

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IPHelper.GetIPAddress();

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("uid", usuario.Id),
                new Claim("ip", ipAddress),

            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtsettings.Issuer,
                audience: _jwtsettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtsettings.DurationInMinutes),
                signingCredentials: signInCredentials
                );

            return jwtSecurityToken;

        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIP = ipAddress
            };
        }

        private string RandomTokenString()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(40);
            return Convert.ToHexString(randomBytes);
        }
    }
}
