﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PictlData.Models
{
    public class User
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
