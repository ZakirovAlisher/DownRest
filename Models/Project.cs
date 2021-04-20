using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Project
    {
       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [System.Web.Mvc.Remote("CheckReward", "Home", ErrorMessage = "Reward is not valid.")]
        public int Reward { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int? CatId { get; set; }
        public Category Cat { get; set; }


        public int AcceptedBy { get; set; } = 0;

        public ICollection<Response> Responses { get; set; }
        public Project()
        {
            AcceptedBy = 0;
        }

    }
}