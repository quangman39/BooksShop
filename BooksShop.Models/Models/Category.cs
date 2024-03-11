﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksShop.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1,100)]
        public uint DisplayOrder { get; set; }
    }
}
