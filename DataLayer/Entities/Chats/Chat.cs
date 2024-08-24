using DataLayer.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chats
{
    public class Chat : BaseEntities
    {
        public string ChatBody { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        #region Relation
        [ForeignKey("UserId")]
        public UserChat UserChat { get; set; }
        [ForeignKey("GroupId")]
        public ChatGroup ChatGroup { get; set; }
        #endregion
    }
}
