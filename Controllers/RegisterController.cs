using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace RegisterAndLoginServices.Controllers
{
    [ApiController]
    public class RegisterController : ControllerBase
    {
        readonly string user = "user";
        readonly string admin = "admin";
        /// <summary>
        ///     提交注册信息 API
        /// </summary>
        /// <param name="register">注册信息模型</param>
        /// <returns>注册成功返回用户 ID，注册失败返回 error</returns>
        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Submit([FromBody] object registerString)
        {
            RegisterModel register;

            try
            {
                JObject jo = JObject.Parse(registerString.ToString());
                register = jo.ToObject<RegisterModel>();
            }
            catch
            {
                return BadRequest(new { error = "Invalid input" });
            }

            var info = new RegisterModel
            {
                password = register.password,
                contact = register.contact,
                userType = user
            };

            try
            {
                return CreatedAtRoute(this.ControllerContext, new
                {
                    message = "Success",
                    id = Register.AccepteRegister(info)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        ///     创建管理员账号
        /// </summary>
        /// <param name="register">注册信息模型</param>
        /// <returns>注册成功返回用户 ID，注册失败返回 error</returns>
        [Route("api/[controller]/admin")]
        [HttpPost]
        public IActionResult Admin([FromBody] object registerString)
        {
            var auth = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Request.Headers["authorization"].ToString().Split(' ')[1]);
            var role = auth.Claims.First(t => t.Type.Equals(ClaimTypes.Role))?.Value;
            if (role is not "suadmin")
            {
                return Forbid();
            }

            RegisterModel register;
            try
            {
                JObject jo = JObject.Parse(registerString.ToString());
                register = jo.ToObject<RegisterModel>();
            }
            catch
            {
                return BadRequest(new { error = "Invalid input" });
            }

            var info = new RegisterModel
            {
                password = register.password,
                contact = register.contact,
                userType = admin
            };

            try
            {
                return CreatedAtRoute(this.ControllerContext, new
                {
                    message = "Success",
                    id = Register.AccepteRegister(info)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
