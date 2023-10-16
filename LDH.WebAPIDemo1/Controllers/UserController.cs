

using LDH.WebAPIDemo1.Model;
using LDH.WebAPIDemo1.Model.Result;
using LDH.WebAPIDemo1el.Model.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LDH.WebAPIDemo1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]

    [Authorize(policy: "customPolicy")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ResultApi GetUser(int id)
        {
            User user = new()
            {
                Id = id,
                Account = "luo" + "MA",
                Name = "luo",
                Role = "admin",
                Email = "1152417278@qq.com",
                LoginTime = DateTime.Now,
                Password = "123456" + "K8S"
            };


            return ResultHelper.Success(user);
            
        }
    }
}
