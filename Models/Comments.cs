using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wall.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Must provide a comment")]
        [MinLength(8, ErrorMessage="Must be 8 characters or long to be a valid comment")]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("user")]
        public int User_Id { get; set; }
        public User user { get; set; }

        [ForeignKey("message")]
        public int Messages_Id { get; set; }
        public Messages message { get; set; }

    }
}