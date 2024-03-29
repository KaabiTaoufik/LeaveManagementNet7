﻿using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.web.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }

        [Display(Name = "Leave Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Leave Description")]
        public string? Description { get; set; }

        [Display(Name = "Deafult Number Of Days")]
        [Required]
        public int DefaultDays { get; set; }
    }
}
