using JWTIdentity.API.Entities;
using JWTIdentity.API.Models;
using JWTIdentity.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<AppUser> userManager,
                                IJwtService jwtService,
                                RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname
            };

            if (model.Password == null)
            {
                return BadRequest("Şifre boş olamaz");
            }

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var isRoleExist = await roleManager.RoleExistsAsync("Admin");
            if (!isRoleExist)
            {
                var role = new AppRole
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(role);
            }

            await userManager.AddToRoleAsync(user, "Admin");

            return Ok("Kullanıcı Kaydı Başarılı");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return BadRequest("Şifre hatalı");
            }

            var token = await jwtService.CreateTokenAsync(user);
            return Ok(token);
        }
    }
}
