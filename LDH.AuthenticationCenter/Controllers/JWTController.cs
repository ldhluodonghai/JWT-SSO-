using LDH.AuthenticationCenter.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Result;

namespace LDH.AuthenticationCenter.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly IJWTService _IJWTService;
        public JWTController(IJWTService iJWTService)
        {
            _IJWTService = iJWTService;
        }
        [HttpPost]
        public ResultApi GetToken(string name,string password)
        {
            if(string.IsNullOrEmpty(name)||string.IsNullOrEmpty(password))
            {
                return  ResultHelper.Error("空");
            }
            if("luo".Equals(name, StringComparison.OrdinalIgnoreCase)&&"123456".Equals(password))
            {
                //生成JWT
                string token  = this._IJWTService.GetToken(name);
                return ResultHelper.Success(token);
            }
            return ResultHelper.Error("用户信息错误，获取失败");

        } 
        [HttpPost]
        public ResultApi GetToken1(string name,string password)
        {
            if(string.IsNullOrEmpty(name)||string.IsNullOrEmpty(password))
            {
                return  ResultHelper.Error("空");
            }
            if("luo".Equals(name, StringComparison.OrdinalIgnoreCase)&&"123456".Equals(password))
            {
                //生成JWT
                string token  = this._IJWTService.GetToken(name);
                return ResultHelper.Success(token);
            }
            return ResultHelper.Error("用户信息错误，获取失败");

        }
    }
}
