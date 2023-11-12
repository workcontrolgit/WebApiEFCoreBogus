using OnionApiUpgradeBogus.Domain.Common;
using OnionApiUpgradeBogus.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class Employee : AuditableBaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        public string EmployeeTitle { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; } = default!;
        public Gender Gender { get; set; }
        public string EmployeeNumber { get; set; }
        public string Suffix { get; set; }
        public string Phone { get; set; }
    }
}