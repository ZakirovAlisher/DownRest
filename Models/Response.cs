using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Response
    {
        public Response(int id, string text, int? projectId, Project project, int? freelancerId, Freelancer freelancer)
        {
            Id = id;
            Text = text;
            ProjectId = projectId;
            Project = project;
          //  FreelancerId = freelancerId;
          //  Freelancer = freelancer;
        }
        public Response()
        {

        }
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }

        public int? ProjectId { get; set; }

        public Project Project { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}