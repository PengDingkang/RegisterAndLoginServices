using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RegisterAndLoginServices.Models;

namespace RegisterAndLoginServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        ///     提交登录信息验证
        /// </summary>
        /// <param name="login">登录信息模型</param>
        /// <returns>提交登录信息，返回 token</returns>
        [HttpPost]
        public IActionResult Submit([FromBody] LoginModel login)
        {
            int userId;
            string userType;
            try
            {
                // 调用密码验证的方法，若密码不正确或其他输入不正确等情况抛出异常
                (userId, userType) = GlobalFunctions.VerifyPassword(Convert.ToInt64(login.id), login.password);
            }
            catch (FormatException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }

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
                userType,
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
