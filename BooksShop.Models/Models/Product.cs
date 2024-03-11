using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2,ErrorMessage ="must lagre than 2 letter ")]
        public string? Title { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        [Required]
        public string? Author { get; set; }

        [Required]
        public string? ISBN { get; set; }

        [Required]
        [Display(Name="List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "List Price 1 - 50")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "List Price 50+")]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "List Price 100+")]
        public double Price100 {  get; set; }

        [ValidateNever]
        public int CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category category { get; set; }

        [Display(Name ="Image")]
        public string? image { get; set; }
    }
}
