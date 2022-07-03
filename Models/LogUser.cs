using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpWeddingPlanner.Models
{
    public class LogUser
    {
    [NotMapped]

    [Required]
    [EmailAddress]
    public string LoginEmail {get; set;}

    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Must be 8 characters or longer!")]
    public string LoginPassword {get; set;}
    }
}