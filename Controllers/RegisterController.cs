﻿using Microsoft.AspNetCore.Mvc;
using System;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;

namespace RegisterAndLoginServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        ///     提交注册信息 API
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="contact">联系方式</param>
        /// <returns>注册成功返回用户 ID，注册失败返回 error</returns>
        [HttpPost]
        public IActionResult Submit([FromQuery] string contact, [FromQuery] string password)
        {
            var info = new RegisterModel
            {
                password = password,
                contact = contact,
                userType = "user"
            };
            try
            {
                return Ok(new { 
                    message = "Success", id = Register.AccepteRegister(info) 
                });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }
    }
}
