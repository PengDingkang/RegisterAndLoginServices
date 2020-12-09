using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;

namespace RegisterAndLoginServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        readonly string user = "user";
        /// <summary>
        ///     提交注册信息 API
        /// </summary>
        /// <param name="register">注册信息模型</param>
        /// <returns>注册成功返回用户 ID，注册失败返回 error</returns>
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
    }
}
