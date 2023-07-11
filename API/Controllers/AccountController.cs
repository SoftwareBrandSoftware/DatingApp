using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTO;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDto)
        {

            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");  

               using var hmac = new HMACSHA512(); // will provide us with our hashing algorithm Implement the Idisposible Interface if we press f12 with our curos with the class itself it will take us to the metadata of the class describes description about what it does... inherits from a class called Hmac constructor takes a key as byte ray... has a property as a key  , has an initilizae method and has a Dispose method.... released maneged resources... , time to go up inherit chain HMACSHA512 inherits from HMAc and HMAC inherits from KeyedHasedAlgorithm, KeyedHashedAlgorithm which inherits from HashAlgorithm which this one inherits from IDispose and such will need to use the Idspose method from the HMACSHA512() method  so now we need to program it manaully 
               
               var user = new AppUser
               {
                   UserName = registerDto.Username.ToLower(),                                           
                   PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                   PasswordSalt = hmac.Key
               };
               _context.Users.Add(user);
               await _context.SaveChangesAsync();

               return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length ; i ++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }

}