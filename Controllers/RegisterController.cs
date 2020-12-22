using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;

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
        public IActionResult Submit([FromBody] RegisterModel register)
        {
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
                    message = "Success", id = Register.AccepteRegister(info)
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
        [Authorize]
        [HttpPost]
        public IActionResult Admin([FromBody] RegisterModel register)
        {
            var auth = HttpContext.AuthenticateAsync();
            var role = auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Role))?.Value;
            if(role is not "suadmin")
            {
                return Forbid();
            }
            var info = new RegisterModel
            {
                // TODO: Consider sending a random password with email.
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
