using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wall.Models
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Must provide a message")]
        [MinLength(8, ErrorMessage="Must be 8 characters or long to be a valid message")]
        public string Message { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set;}

        [ForeignKey("user")]
        public int User_Id { get; set; }
        public User user { get; set; }

        public List<Comments> Comments { get; set; }
        public Messages()
        {
            Comments = new List<Comments>();
        }
    }
}