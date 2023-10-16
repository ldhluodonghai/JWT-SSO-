using Microsoft.AspNetCore.Authorization;

namespace LDH.WebAPIDemo1.Authorized
{
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public CustomAuthorizationRequirement(string policyname)
        {
            this.Name = policyname;
        }

        public string Name { get; set; }
    }
}
