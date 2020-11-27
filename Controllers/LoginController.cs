﻿using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RegisterAndLoginServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        ///     提交登录信息验证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns>提交登录信息，返回 token</returns>
        [HttpPost]
        public IActionResult Submit([FromQuery] string id, [FromQuery] string password)
        {
            Console.WriteLine(GlobalVars.domain);
            return GetToken(id, password);
        }

        /// <summary>
        ///     私有方法，获取 JWT Token
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>Token</returns>
        private IActionResult GetToken(string id, string password)
        {
            int userId;
            string userType;
            try
            {
                // 调用密码验证的方法，若密码不正确或其他输入不正确等情况抛出异常
                (userId, userType) = GlobalFunctions.VerifyPassword(Convert.ToInt64(id), password);
            }
            catch (Exception e)
            {
                return Ok(new { Error = e.Message });
            }

            Console.WriteLine(userId + userType);

            // 生成 token 的 payload 部分
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp,
                    $"{new DateTimeOffset(DateTime.Now.AddHours(1)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), //自定义的 payload 部分，包含了用户的 ID 用于识别身份
                new Claim(ClaimTypes.Role, userType)
            };
            // 生成密钥用于签名
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalVars.secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 生成 token
            var token = new JwtSecurityToken(
                GlobalVars.domain,
                GlobalVars.domain,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            // 返回 token 给客户端使用
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
