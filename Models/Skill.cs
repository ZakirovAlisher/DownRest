using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Skill : IValidatableObject
    { 
        public int Id { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public Skill()
        {
            Users = new List<User>();
        }
       
        public Skill(int id, string name, ICollection<User> freelancers)
        {
            Id = id;
            Name = name;
            Users = freelancers;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                errors.Add(new ValidationResult("Введите название скилла [IValidatableObject]"));
            } 

            return errors;
        }
    }
}