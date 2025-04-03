using System.ComponentModel.DataAnnotations;

namespace SimpleApi.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid Id{get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public string Description{get;set;}
    }
}