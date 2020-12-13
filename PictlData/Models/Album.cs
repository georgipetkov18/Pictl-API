using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PictlData.Models
{
    public class Album
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public ICollection<Photo> Photos { get; set; }

        public Album()
        {
            this.Photos = new HashSet<Photo>();
        }
    }
}
