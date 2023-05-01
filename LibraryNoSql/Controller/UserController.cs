using LibraryNoSql.ApiModel;
using LibraryNoSql.Model;
using LibraryNoSql.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryNoSql.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;
        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserApiModel model)
        {
            var existing = userRepository.GetByLogin(model.Login);
            if (existing != null)
                return BadRequest(new
                {
                    Error = "User already exist"
                });
            var dbUser = userRepository.Insert(new User()
            {
                Login = model.Login,
                Password = model.Password
            });
            return Ok(dbUser);
        }
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var users = userRepository.GetAll();
            return Ok(users);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserApiModel model)
        {
            var user = userRepository.GetByLoginAndPassword(model.Login, model.Password);
            if (user == null)
                return BadRequest(new
                {
                    Error = "User does not exist"
                });
            var identity = GetIdentity(user.Login, "User");
            var token = JWTTokenizer.GetEncodedJWT(identity, AuthOptions.Lifetime);
            return new JsonResult(new
            {
                JWT = token
            });
        }
        private ClaimsIdentity GetIdentity(string login, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            ClaimsIdentity claimsIdentity = new
            ClaimsIdentity(claims, "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
