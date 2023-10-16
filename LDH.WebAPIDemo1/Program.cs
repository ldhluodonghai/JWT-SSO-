
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
                    Description = "ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
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

            //����JWT�ļ�Ȩ
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            builder.Configuration.Bind("JWTTokenOptions", tokenOptions);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = tokenOptions.Audience,//
                        ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//�õ�SecurityKey 
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
                //������Ȩ����
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
            app.UseAuthentication();//�������ˣ�һ��Ҫ�ȼ�Ȩ--�����û�ƾ֤��
            app.UseAuthorization();//��Ȩ���


            app.MapControllers();

            app.Run();
        }
    }
}