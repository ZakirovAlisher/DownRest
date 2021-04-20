using DownRest.Models;
using System.ComponentModel.DataAnnotations;

namespace DownRest.Annotations
{
    public class SurnameAttribute : ValidationAttribute
    {



        public override bool IsValid(object value)
        {
            Freelancer b = value as Freelancer;
            if (b.Name == "Adolf" && b.Surname == "Hitler")
            {
                return false;
            }
            return true;
        }
    }
}