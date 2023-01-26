using System.ComponentModel.DataAnnotations;

namespace dotnetEtsyApp.Models.RecordsData
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int CreatedUser { get; set; } 
        public int UpdatedUser { get; set; }

        
    }
}