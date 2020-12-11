using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PictlData.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public ICollection<CategoryPhoto> CategoryPhotos { get; set; }

        //public Category()
        //{
        //    this.Photos = new HashSet<Photo>();
        //}
    }
}
