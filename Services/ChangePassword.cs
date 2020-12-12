using System;
using RegisterAndLoginServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterAndLoginServices.Services
{
    public class ChangePassword
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="idi">读者 ID</param>
        /// <param name="oldPw">原密码</param>
        /// <param name="newPw">新密码</param>
        public static void UserPasswordChange(int idi, string oldPw, string newPw)
        {
            if (!GlobalFunctions.IsValidPassword(newPw))
            {
                throw new Exception("Password Invalid");
            }
            try
            {
                GlobalFunctions.VerifyPassword(idi, oldPw);
            }
            catch (Exception)
            {
                throw;
            }
            string salt = Guid.NewGuid().ToString();
            string pwHash = GlobalFunctions.EncryptPassword(newPw, salt);
            RegisterModel newInfo = new RegisterModel
            {
                id = idi,
                passwordHash = pwHash,
                salt = salt
            };
            var result = DbContext.DBstatic.Updateable(newInfo).UpdateColumns(it => new { it.salt, it.passwordHash }).ExecuteCommand();
            try
            {
                GlobalFunctions.VerifyPassword(idi, newPw);
            }
            catch (Exception) {
                throw new Exception("Failed Change");
            }

        }
    }
}
