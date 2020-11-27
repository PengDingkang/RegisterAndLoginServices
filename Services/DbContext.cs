using System;
using System.Linq;
using SqlSugar;
using RegisterAndLoginServices.Models;

namespace RegisterAndLoginServices.Services
{
    public class DbContext
    {
        //注意：不能写成静态的，不能写成静态的
        public SqlSugarClient Db; //用来处理事务多表查询和复杂的操作


        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = GlobalVars.connStr,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            //调试代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                                  Db.Utilities.SerializeObject(
                                      pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }

        public SimpleClient<User> ReaderDb => new SimpleClient<User>(Db); //用来处理ReaderInfo表的常用操作

        //静态调用的写法
        public static SqlSugarClient DBstatic =>
            new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = GlobalVars.connStr,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
    }
}
