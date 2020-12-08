using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PictlData.Models
{
    public class Photo
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public ICollection<Category> Categories { get; set; }

        public int Likes { get; set; }

        public Photo()
        {
            this.Categories = new HashSet<Category>();
        }
    }
}
