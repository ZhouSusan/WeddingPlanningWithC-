using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSharpWeddingPlanner.Models
{
    public class Wedding
    {    
    [Key]
    public int WeddingId {get; set;}

    [Required]
    [MinLength(2, ErrorMessage ="Must be at least 2 characters long")]
    public string WedderOne {get; set;}

    [Required]
    [MinLength(2, ErrorMessage ="Must be at least 2 characters long")]
    public string WedderTwo {get; set;}

    [Required]
    [FutureDateAttribute(ErrorMessage ="Must be a future date")]
    public DateTime Date {get; set;}
    
    [Required]
    [MinLength(2, ErrorMessage ="Must be at least 2 characters long")]
    public string WeddingAddress {get; set;}

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

    public List<GuestResponse> GuestReponses {get; set; }
    public int PlannerId {get; set;}
    public User Planner {get; set;}
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            } 
            DateTime date = (DateTime)value;

            if (date <= DateTime.Now)
            {
                return new ValidationResult("Date must be set in the future.");
            }
            return ValidationResult.Success;
        }
    }
}