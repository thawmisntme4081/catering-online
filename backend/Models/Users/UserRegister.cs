﻿using System.ComponentModel.DataAnnotations;
using static backend.Models.User;

namespace backend.Models {
    public class UserRegister {
        [Required]
        [Range(0, 2)]
        public UserType Type { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 4)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string FirstName { get; set; }
        [StringLength(255, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8)]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Address { get; set; }
        public UserRegister() {
            Type = 0;
            Email = "";
            Password = "";
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            Address = "";
        }
    }
}
