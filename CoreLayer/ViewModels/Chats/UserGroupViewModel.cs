using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ViewModels.Chats
{
    public class UserGroupViewModel
    {
        public string GroupName { get; set; }
        public string token { get; set; }
        public Chat LastChat { get; set; }
    }
}
