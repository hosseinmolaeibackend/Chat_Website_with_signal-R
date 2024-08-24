using DataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Roles
{
    public class RolePermission:BaseEntities
    {

        public int RoleId {  get; set; }
        public Permission Permission { get; set; }

        #region Relation
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        #endregion
    }
}
