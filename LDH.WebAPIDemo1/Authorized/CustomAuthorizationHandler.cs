using Microsoft.AspNetCore.Authorization;

namespace LDH.WebAPIDemo1.Authorized
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        public CustomAuthorizationHandler() { }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {

            bool flag = false;

            //把context转换到httpConext,方便取上下文
            HttpContext httpContext = context.Resource as HttpContext;
            string path = httpContext.Request.Path;//当前访问路径，例："/Home/roleData4"

            var user = httpContext.User;
            //用户id
            string userId = user.FindFirst("id")?.Value;
            if (userId == null)
            {
                //没登录，直接结束
                return Task.CompletedTask;
            }

            //登录成功时根据角色查出来这个用户的全部角色的菜单权限中的url地址，存到redis,这里实际是根据用户id从redis查询出来url地址。
            List<string> paths = new List<string>()
            {
                "/Home/roleData4",
                "/Home/roleData3"
            };

            if (requirement.Name == "Policy01")
            {
                Console.WriteLine("进入自定义策略授权01...");
                ///策略1的逻辑
                if (paths.Contains(path))
                {
                    flag = true;
                }
            }

            if (requirement.Name == "Policy02")
            {
                Console.WriteLine("进入自定义策略授权02...");
                ///策略2的逻辑
            }

            if (flag)
            {
                context.Succeed(requirement); //验证通过了
            }

            return Task.CompletedTask; //验证不同过
        }
    }
}
