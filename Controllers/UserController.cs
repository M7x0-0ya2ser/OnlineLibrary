using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.DTOs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace OnlineLibrary.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository<User> _repository;

        public UserController(IDataRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.GetAllAsync();
            return Ok(users);
        }

        //[HttpPut]

        //public async Task<IActionResult> UpdateUser(User user)
        //{
        //    var existinguser = await _repository.GetByIdAsync(user.Id);

        //    if (existinguser is null)
        //    {
        //        return NotFound("There is no user with this id");
        //    }

        //    _repository.Update(user);
        //    await _repository.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("No User With This Id");
            }
            return Ok(user);
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id,[FromBody] updateuser dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if(dto.action== 0)
            {
                user.Isaccepted= true;
            }
            else
            {
                user.Isaccepted= false;
            }
            _repository.Update(user);
            await _repository.SaveChangesAsync();
            return Ok();
        }

    }
}
