using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Resume
    {
        public Resume(int id, string text, IEnumerable<Freelancer> freelancers)
        {
            Id = id;
            Text = text;
            Freelancers = freelancers;
        }
        public Resume( )
        {
            
        }
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }

        public IEnumerable<Freelancer> Freelancers { get; set; }
    }
}