using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterAndLoginServices.Models
{
    public class UserInfoModel
    {
        public int id { get; set; }
        public string contact { get; set; }
        public string userType { get; set; }
        public string avatar { get; set; }
    }
}
