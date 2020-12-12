using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace RegisterAndLoginServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        /// <summary>
        ///     修改密码
        /// </summary>
        /// <param name="jsonStr">新密码和旧密码的 json</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult ChangePassword ([FromBody] object jsonStr)
        {
            int id;
            ChangePasswordModel model;
            try
            {
                var auth = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Request.Headers["authorization"].ToString().Split(' ')[1]);
                id = Convert.ToInt32(auth.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Unauthorized API call" });
            }

            try
            {
                JObject jo = JObject.Parse(jsonStr.ToString());
                model = jo.ToObject<ChangePasswordModel>();
            }
            catch
            {
                return BadRequest(new { error = "Invalid input" });
            }

            try
            {
                Services.ChangePassword.UserPasswordChange(id, model.oldPw, model.newPw);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = $"Password change Failed: {e.Message}" });
            }
            return NoContent();
        }
    }
}
