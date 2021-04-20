using DownRest.Annotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    [Surname(ErrorMessage = "Осуждаю!")]
    public class Freelancer
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
       
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3 до 50 символов")]
        public string Password { get; set; }

        public int ResumeId { get; set; }

        public virtual Resume Resume { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
       public Freelancer()
       {
           Skills = new List<Skill>();
       }

        public Freelancer(int id, string name, string surname, string email, string password, int resumeId, Resume resume, ICollection<Skill> skills)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            ResumeId = resumeId;
            Resume = resume;
            Skills = skills;
        }
    }
}