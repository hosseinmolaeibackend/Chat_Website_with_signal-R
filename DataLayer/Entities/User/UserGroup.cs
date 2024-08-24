using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.User
{
    public class UserGroup:BaseEntities
    {
        public int userId {  get; set; }
        public int groupId { get; set; }

        #region relations
        [ForeignKey("userId")]
        public UserChat? UserChat { get; set; }
        [ForeignKey("groupId")]
        public  ChatGroup? ChatGroup { get; set; }
        #endregion
    }
}
