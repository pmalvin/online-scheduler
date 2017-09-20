using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineScheduler.Models
{
    public class User
    {
        [Key]
        public string EmailAddress { get; set; }

        [MaxLength(30)]
        public string UserName { get; set; }
        public DateTimeOffset RegisterDate { get; set; }

        [MaxLength(30), MinLength(10)]
        public string PassWord { get; set; }
    }
}