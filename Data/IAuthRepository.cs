using Microsoft.EntityFrameworkCore;
using OnlineLibrary.DTOs;
using OnlineLibrary.Models;

namespace Online_Library.Data
{
    public interface IAuthRepository
    {
        void Register(UserRegisterDto userDto);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
        User CheckForExistingUsers(UserRegisterDto user);
        public User GetUserByEmail(UserLoginDto user);
        
    }
}
