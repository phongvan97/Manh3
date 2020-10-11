using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LamDep.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
    }
}