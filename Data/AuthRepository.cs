using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineLibrary.DTOs;
using OnlineLibrary.Models;
using OnlineLibrary.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Online_Library.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly OnlineLibraryContext _context;
        public AuthRepository(IMapper mapper, IConfiguration configuration, OnlineLibraryContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }
        public string CreateToken(User user)
        {
            string Role;
            if (user.Isadmin is true)
            {
                Role = "Admin";
            }
            else
            {
                Role = "User";
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,Role),
                new Claim(ClaimTypes.Email, user.Email)


        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public  void Register(UserRegisterDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Isaccepted = null;
            user.Isadmin = false;
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Passwordhash = passwordHash;
            user.Passwordsalt = passwordSalt;
            // auto accept new user and make them admin if there is no users in DB
            bool anyUsers = _context.Users.Any();
            if (!anyUsers)
            {
                user.Isaccepted = true;
                user.Isadmin = true;
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public User CheckForExistingUsers(UserRegisterDto user)
        {
            string userName = user.Username;
            string email = user.Email;
            var existingUser = _context.Users.Where(e => e.Username == userName || e.Email == email).FirstOrDefault();

            return existingUser;
        }

        public User GetUserByEmail(UserLoginDto user)
        {
            string email = user.Email;
            var existingUser = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            return existingUser;
        }


    }
}
