using SqlSugar;

namespace RegisterAndLoginServices.Models
{
    [SugarTable("user")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string contact { get; set; }
        public string passwordHash { get; set; }
        public string salt { get; set; }
        [SugarColumn(DefaultValue = "user")]
        public string userType { get; set; }

        /// <summary>
        /// 重载解构函数，返回 ID 和用户类型
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <param name="userTpye">用户类型</param>
        public void Deconstruct(out int id, out string userTpye)
        {
            id = this.id;
            userTpye = this.userType;
        }
    }
}
