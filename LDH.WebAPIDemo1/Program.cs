
using LDH.WebAPIDemo1.Authorized;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace LDH.WebAPIDemo1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                 Id="Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //配置JWT的鉴权
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            builder.Configuration.Bind("JWTTokenOptions", tokenOptions);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = tokenOptions.Audience,//
                        ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//拿到SecurityKey 
                    };
                });
           /* builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ComplicatedPolicy", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.Name)
                    .RequireClaim(ClaimTypes.Role)
                    .RequireClaim(ClaimTypes.Email)
                    .RequireAssertion(context =>
                    {
                        return context.User.Claims.Any(c => c.Type.Equals(ClaimTypes.Email))
                        && context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))!.Value.EndsWith(".net");
                    });
                });
            });*/
            builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            builder.Services.AddAuthorization(optins =>
            {
                //增加授权策略
                optins.AddPolicy("customPolicy", polic =>
                {
                    polic.AddRequirements(new CustomAuthorizationRequirement("Policy01")
                         ,new CustomAuthorizationRequirement("Policy02")
                        );
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();//请求来了，一定要先鉴权--解析用户凭证；
            app.UseAuthorization();//授权检测


            app.MapControllers();

            app.Run();
        }
    }
}