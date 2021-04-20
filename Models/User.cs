using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class User : IdentityUser
    {
        
     
        public int balance { get; set; }
        public string avatar { get; set; }
        public string role { get; set; } = "user";
        public virtual ICollection<Skill> Skills { get; set; }



        public User()
        {
            Skills = new List<Skill>();
        }
        
    }
}