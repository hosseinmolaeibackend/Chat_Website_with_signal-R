using DataLayer.Entities.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.User
{
    public class UserRole : BaseEntities
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        #region Relation
        [ForeignKey("UserId")]
        public UserChat UserChat { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        #endregion
    }
}
