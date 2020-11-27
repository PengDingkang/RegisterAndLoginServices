using System;
using RegisterAndLoginServices.Models;
using MySql.Data.MySqlClient;

namespace RegisterAndLoginServices.Services
{
    public class Register
    {
        /// <summary>
        ///     接受注册信息，并将用户信息写入数据库
        /// </summary>
        /// <param name="ls">注册信息参数列表</param>
        /// <exception cref="MySqlException"></exception>
        /// <exception cref="Exception"></exception>
        /// <returns>用户 ID</returns>
        public static int AccepteRegister(RegisterModel reg)
        {
            try
            {
                _ = GlobalFunctions.CheckRegisterInput(reg);
            }
            catch (Exception)
            {
                throw;
            }

            if (reg != null)
            {
                User newReader;
                reg.salt = Guid.NewGuid().ToString();
                reg.passwordHash = GlobalFunctions.EncryptPassword(reg.password, reg.salt);
                newReader = reg;
                try
                {
                    var id = DbContext.DBstatic.Insertable(newReader).ExecuteReturnIdentity();
                    Console.WriteLine("注册成功");
                    return id;
                }
                catch (MySqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            throw new Exception("Failed to register");
        }
    }
}
