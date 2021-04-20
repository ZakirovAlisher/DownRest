using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DownRest.Models
{
    public class Client
    {
        public Client(int id, string name, string surname, string email, string password, ICollection<Project> projects)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
           
        }
        public Client()
        {
           
        }

        public int Id { get; set; }
        [Required]
        [Remote("CheckName", "Clients", ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3 до 50 символов")]
        public string Password { get; set; }

        
    }
}