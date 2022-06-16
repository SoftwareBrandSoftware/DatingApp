using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

// Note we are using Angular for the V of the MVC the view will be the Angular part
namespace API.Controllers
{
    [ApiController]                                                                 //specifying this code is a Api Controller.
    [Route("api/[controller]")]                                                    // specifying Routing of Api of how we get to here example api/Users
    public class UsersController : ControllerBase                                  //Inherite from ControllerBase with the dotnet MVC framework
    {
                                                                                   //to get data from our databse we need to use dependency injection ..the  following is below
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()        //using API.entities -->AppUser ,  using Systems collections.Genric--> IEnumerable
        {
            return await _context.Users.ToListAsync();                    // ToListAsync uses Microsoft.EtntityFrameworkCore
        }
      
        //when someone hits this endpoint effectively we are saying api/users/3  <--for id of 3 for the user we are fetching from the database
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)        // we are geting the user with the integer ID of some number. async Task< > added for scalability
        {
            return await _context.Users.FindAsync(id);                         //We use Find Async method to get the data we have the id used
        }
    }
}