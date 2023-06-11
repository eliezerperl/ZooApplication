using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooDAL.Entities
{
    public class Animal
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Age { get; set; }
        
        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Description { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public Guid CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
