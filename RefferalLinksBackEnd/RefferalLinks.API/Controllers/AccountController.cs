using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        ILoginService _iloginService;

        public AccountController(ILoginService iloginService)
        {
            _iloginService = iloginService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserModel login)
        {
            var result = await _iloginService.AuthenticateUser(login);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Regisger(UserModel login)
        {
            var result = await _iloginService.CreateUser(login);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _iloginService.LogoutUser();
            return Ok(result);
        }
    }
}
