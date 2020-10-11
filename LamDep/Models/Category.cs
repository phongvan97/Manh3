using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LamDep.Models
{
    public class Category
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}