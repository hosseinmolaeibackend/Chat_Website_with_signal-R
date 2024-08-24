using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chats
{
    public class ChatGroup:BaseEntities
    {
        public string GroupTitle { get; set; }
        public string GroupToken { get; set; }

        public int OwnerId { get; set; }


        #region Relation
        [ForeignKey("OwnerId")]
        public UserChat User { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        #endregion
    }
}
