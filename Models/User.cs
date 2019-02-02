using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wall.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get ; set; }
        public string LastName { get ; set; }
        public string Email { get ; set; }

        [DataType(DataType.Password)]
        public string Password { get ; set; }

        public List<Messages> Messages { get; set; }

        public List<Comments> Comments { get; set; }
        public User()
        {
            Messages = new List<Messages>();
            Comments = new List<Comments>();
        }
    }
}