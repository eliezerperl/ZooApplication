using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities.Interface;

namespace ZooDAL.Entities
{
    public class Comment : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Content field is required.")]
        public string Content { get; set; }
        public Guid AnimalID { get; set; }
        public Animal Animal { get; set; }
    }
}
