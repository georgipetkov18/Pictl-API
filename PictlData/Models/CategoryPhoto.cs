using System;
using System.Collections.Generic;
using System.Text;

namespace PictlData.Models
{
    public class CategoryPhoto
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
