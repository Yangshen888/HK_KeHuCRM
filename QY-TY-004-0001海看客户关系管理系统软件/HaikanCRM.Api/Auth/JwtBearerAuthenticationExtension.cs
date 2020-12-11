using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace HaikanCRM.Api.Auth
{
    /// <summary>
    /// jwt工具类
    /// </summary>
    public static class JwtBearerAuthenticationExtension
    {
        /// <summary>
        /// 注册JWT Bearer认证服务的静态扩展方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings">JWT授权的配置项</param>
        public static void AddJwtBearerAuthentication(this IServiceCollection services, AppAuthenticationSettings appSettings)
        {
            //使用应用密钥得到一个加密密钥字节数组
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(cfg => cfg.SlidingExpiration = true)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="claimsIdentity"></param>
        /// <returns></returns>
        public static string GetJwtAccessToken(AppAuthenticationSettings appSettings, ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 创建token,
        /// </summary>
        /// <param name="appSettings">配置项</param>
        /// <param name="claimsIdentity"></param>
        /// <param name="day">token有效天数</param>
        /// <returns></returns>
        public static string GetJwtAccessToken(AppAuthenticationSettings appSettings, ClaimsIdentity claimsIdentity,double day)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(day),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
