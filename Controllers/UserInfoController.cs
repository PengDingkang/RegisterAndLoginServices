using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;
using System.IdentityModel.Tokens.Jwt;

namespace RegisterAndLoginServices.Controllers
{
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        /// <summary>
        ///     通过 Token 或 Query 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("api/[controller]")]
        [HttpGet]
        public async Task<IActionResult> GetInfo([FromQuery] string id = null)
        {
            var auth = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Request.Headers["authorization"].ToString().Split(' ')[1]);
            id ??= auth.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (id == auth.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value
                || auth.Claims.First(t => t.Type.Equals(ClaimTypes.Role))?.Value is "admin" or "suadmin")
            {
                return await QueryUserInfo(id);
            }
            else return Forbid();
        }

        /// <summary>
        ///     通过 URL 参数获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/[controller]/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetInfoById([FromRoute] string id)
        {
            var auth = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Request.Headers["authorization"].ToString().Split(' ')[1]);
            id ??= auth.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (id == auth.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value
                || auth.Claims.First(t => t.Type.Equals(ClaimTypes.Role))?.Value is "admin" or "suadmin")
            {
                return await QueryUserInfo(id);
            }
            else return Forbid();
        }

        /// <summary>
        ///     通过 ID 获取用户信息模型
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns></returns>
        [NonAction]
        public async Task<IActionResult> QueryUserInfo(string id)
        {
            try
            {
                var user = await DbContext.DBstatic.Queryable<User>().InSingleAsync(id);
                UserInfoModel info = new UserInfoModel
                {
                    id = user.id,
                    contact = user.contact,
                    userType = user.userType,
                    // FIXME: avatar
                    avatar = "No avatar"
                };
                return Ok(info);
            }
            catch (NullReferenceException)
            {
                return NotFound(new { error = "User not exist." });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
