using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Data;
using OnlineLibrary.Data;
using OnlineLibrary.DTOs;

namespace Online_Library.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [AllowAnonymous] // customize token who can enter
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }



        [HttpPost("Register")]

        public IActionResult Register(UserRegisterDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.Password != user.Repassword)
            {
                return BadRequest("Password And Confirm Password Doesn't Match");
            }
            string userName = user.Username;
            string email = user.Email;
            var existingUser = _authRepository.CheckForExistingUsers(user);

            if (!(existingUser == null))
            {
                return BadRequest("Username or email already exists");
            }


            _authRepository.Register(user);
            return CreatedAtAction(nameof(Register), new { user.Email }, user);
        }


        [HttpPost("Login")]

        public IActionResult Login(UserLoginDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = _authRepository.GetUserByEmail(user);

            if (existingUser == null || !_authRepository.VerifyPasswordHash(user.Password, existingUser.Passwordhash, existingUser.Passwordsalt))
            {
                return NotFound("There Is no User With this credntials");
            }
            if (existingUser.Isaccepted is false || existingUser.Isaccepted is null)
            {
                return NotFound("User not Accepted yet");
            }
            string token = _authRepository.CreateToken(existingUser);
            var tokenid = new TokenID();
            tokenid.jwt = token;
            tokenid.id = existingUser.Id;
            return Ok(tokenid);
        }
    }
}
