using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PictlData.Models
{
    public class Photo
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string Url { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public int AlbumId { get; set; }

        [JsonIgnore]
        public ICollection<CategoryPhoto> CategoryPhotos { get; set; }

        [NotMapped]
        public ICollection<string> CategoriesNames { get; set; }

        public int Likes { get; set; }

        public Photo()
        {
            this.CategoriesNames = new HashSet<string>();
        }
    }
}
