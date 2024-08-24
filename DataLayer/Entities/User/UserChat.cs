using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.User
{
    public class UserChat : BaseEntities
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }

        #region Relation
        public ICollection<ChatGroup> Groups { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        #endregion
    }
}
