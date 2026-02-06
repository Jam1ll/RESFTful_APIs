using core.application.DTOs.Users;
using core.application.Enums;
using core.application.Exceptions;
using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Settings;
using infrastructure.identity.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

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
            JwtSecurityToken jwtSecurityToken;

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

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {

        }
    }
}
