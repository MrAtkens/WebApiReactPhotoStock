using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApi.DataAcces;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*", SupportsCredentials = true)]
    public class UsersController : ApiController
    {
        private UsersContext db = new UsersContext();
        const int saltRounds = 4;

        [HttpGet]
        [ActionName("GetUsers")]
        public async Task<IHttpActionResult> GetUsers()
        {
            List<User> users = await db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [ActionName("singIn")]
        public async Task<IHttpActionResult> SingIn(UserDTOs userDTOs)
        {
            try
            {
                User user = await db.Users.Where(element => element.Login == userDTOs.Login).FirstOrDefaultAsync(); ;
                if(user == null)
                {
                    return Ok(new { status = false });
                }
                bool answer = BCrypt.Net.BCrypt.Verify(userDTOs.Password, user.Password);

                if (answer)
                {
                    user.Password = null;
                    user.Id = null;
                    return Ok(new { status = true, userAnswer = user });
                }
                else
                {
                    return Ok(new { status = false });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false });
            }
        }

        [HttpPost]
        [ActionName("singUp")]
        public async Task<IHttpActionResult> SingUp(UserDTOs userDTOs)
        {
            int answer = 0;
            try
            {
                User user = new User() { Login = userDTOs.Login, Password = userDTOs.Password };

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, saltRounds);
                user.Password = hashedPassword;

                User userExist = await  db.Users.Where(element => element.Login == user.Login).FirstOrDefaultAsync(); ;

                if(userExist == null) {
                    db.Users.Add(user);
                    answer = await db.SaveChangesAsync();
                }
                else
                {
                    return Ok(new { status = false });

                }

                if (answer == 1)
                {
                    return Ok(new { status = true });
                }
                //Like conflict for redux 
                return Ok(new { status = false });
            }
            catch (Exception ex)
            {
                //Like conflict for redux 
                return Ok(new { status = false });
            }
        }

    }
}