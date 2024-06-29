﻿using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string NewPassword { get; set; }

        public ChangePasswordDTO()
        {
            OldPassword = "";
            NewPassword = "";
        }
    }
}