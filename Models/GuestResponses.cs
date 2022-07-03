using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpWeddingPlanner.Models
{
    public class GuestResponse
    {
        [Key]
        public int GuestResponseId {get; set;}
        public int UserId {get; set;}
        public int WeddingId {get; set;}

        public User Guest {get; set;}
        public Wedding Wedding {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}