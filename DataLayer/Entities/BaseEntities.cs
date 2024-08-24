using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class BaseEntities
    {
        [Key]
        public int Id { set; get; }
        public DateTime CreateDate { set; get; }
    }
}
