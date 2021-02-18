using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Homework_4_2.API.Data;
using Homework_4_2.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_4_2.API.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class UserController : ControllerBase
        {
            private readonly DummyData _dummyData;
            public UserController(DummyData dummyData)
            {
                _dummyData = dummyData;
            }

            [HttpGet]
            public ActionResult<List<UserModel>> Get()
            {
                return Ok(_dummyData.Users.ToList());
            }

            [HttpGet("{id}")]
            public ActionResult<UserModel> Get(int id)
            {
                var data = _dummyData.Users.FirstOrDefault(c => c.Id == id);
                return (data);
            }

            [HttpPost]
            public ActionResult Post([FromBody] UserModel user)
            {
                _dummyData.Users.Add(user);
                return Ok(user);
            }
        }
    }


