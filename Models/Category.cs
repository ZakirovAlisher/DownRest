using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Category
    {
        public Category(int id, string name, ICollection<Project> projects)
        {
            Id = id;
            Name = name;
            Projects = projects;
        }
        public Category()
        {
            
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}