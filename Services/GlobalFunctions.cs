using RegisterAndLoginServices.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RegisterAndLoginServices.Services
{
    public class GlobalFunctions
    {
        /// <summary>
        ///     判断输入的注册信息是否合法
        /// </summary>
        /// <param name="reg">输入参数列表</param>
        /// <returns>合法则返回去空格后的注册信息清单，否则返回 null</returns>
        /// <exception cref="Exception">若输入不合法，抛出异常</exception>
        public static RegisterModel CheckRegisterInput(RegisterModel reg)
        {
            /*todo: rewrite password check method*/
            try
            {
                InfoCheck(reg, true, reg.password);
            }
            catch (Exception)
            {
                throw;
            }

            return reg;
        }
        /// <summary>
        ///     检查用户信息输入合法性
        /// </summary>
        /// <param name="info">用户信息模型</param>
        /// <param name="checkContactUsed">是否检查手机号重复</param>
        /// <param name="password">注册和修改时检查密码</param>
        /// <param name="checkPassword">是否检查密码合法性</param>
        /// <exception cref="Exception">问题字段</exception>
        public static void InfoCheck(RegisterModel info, bool checkContactUsed, string password = null, bool checkPassword = true)
        {
            try
            {
                if (info.contact != null)
                {
                    IsHandset(info.contact);
                    if (checkContactUsed) HandsetUsed(info.contact);
                }

                if (!string.IsNullOrEmpty(password) || checkPassword) IsValidPassword(password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     校验当前字符串是否符合手机号规范
        /// </summary>
        /// <param name="str_handset">待检验的字符串</param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            if (Regex.IsMatch(str_handset, @"^1[3456789]\d{9}$") && str_handset.Length == 11)
            {
                return true;
            }
            throw new Exception("Invalid phone number");
        }

        /// <summary>
        ///     当前手机号是否已经被使用
        /// </summary>
        /// <param name="contact">手机号</param>
        /// <returns></returns>
        /// <exception cref="Exception">该手机号已被占用</exception>
        public static bool HandsetUsed(string contact)
        {
            var userId = DbContext.DBstatic.Queryable<User>()
                .Select(f => new { f.id, f.contact })
                .Where(it => it.contact == contact).First();
            if (userId == null)
                return false;
            throw new Exception("Handset Used");
        }

        /// <summary>
        ///     检测输入是否为合法的密码
        /// </summary>
        /// <param name="password">输入密码</param>
        /// <returns></returns>
        /// <exception cref="Exception">密码不合法或为空</exception>
        public static bool IsValidPassword(string password)
        {
            var rg = new Regex("^[A-Za-z0-9]{8,24}");
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password can not be null");
            }
            if (rg.IsMatch(password) && password.Length <= 24)
            {
                return true;
            }
            throw new Exception("Invalid password");
        }

        /// <summary>
        ///     将输入的明文密码加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐,请使用 Guid.NewGuid().ToString() 来生成</param>
        /// <returns>密码加盐后的哈希</returns>
        public static string EncryptPassword(string password, string salt)
        {
            var passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        /// <summary>
        ///     验证用户密码是否正确
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="password">输入的密码</param>
        /// <returns></returns>
        public static User VerifyPassword(long userId, string password)
        {
            if (Regex.IsMatch(userId.ToString(), @"^1[3456789]\d{9}$") && userId.ToString().Length == 11)
            {
                userId = FindPersonByContact(userId.ToString());
            }
            else
            {
                try
                {
                    Convert.ToInt32(userId);
                }
                catch
                {
                    throw new FormatException("Invalid input");
                }
            }

            if (!FindPersonById(Convert.ToInt32(userId)))
            {
                throw new Exception($"No such user information {userId} found");
            }

            var getPersonById = DbContext.DBstatic.Queryable<User>().InSingle(userId);
            if (getPersonById.passwordHash == EncryptPassword(password, getPersonById.salt))
            {
                return getPersonById;
            }
            else throw new Exception("Wrong password");
        }

        /// <summary>
        ///     通过手机号判断是否存在该用户
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>用户 ID</returns>
        public static int FindPersonByContact(string contact)
        {
            var userId = DbContext.DBstatic.Queryable<User>()
                .Select(f => new { f.id, f.contact })
                .Where(it => it.contact == contact).First();
            if (userId != null)
                return userId.id;
            throw new Exception($"No such user registered with phone number {contact}");
        }

        /// <summary>
        ///     通过 ID 判断是否存在该用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool FindPersonById(int userID)
        {
            var tmp = DbContext.DBstatic.Queryable<User>().InSingle(userID);
            if (tmp != null) return true;

            return false;
        }
    }
}
