﻿using Contact.Shared.Attributes;
using Contact.Shared.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Shared.Dto
{
    public class PersonalInformationDto
    {
        [FieldValidation("Email")]
        [Required]
        public string? Email { get; set; }
        [FieldValidation("FirstName")]
        [Required]
        public string? FirstName { get; set; }
        [FieldValidation("LastName")]
        [Required]
        public string? LastName { get; set; }
        [FieldValidation("DateofBirth")]
        [BindProperty]
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public GenderType Gender { get; set; }
        [FieldValidation("PhoneNumber")]
        [Required]
        public string? PhoneNumber { get; set; }
        [AllowedExtension([".png", ".jpg"])]
        [MaxFileSize(2 * 1024 * 1024)]
        [Required]
        public IFormFile? Image { get; set; }
        [FieldValidation("PersonalCode")]
        [Required]
        public string? PersonalCode { get; set; }
        [Required]
        public AddressDto? Address { get; set; }

        public PersonalInformationDto() { }

        public PersonalInformationDto(string? email, string? firstName, string? lastName, DateTime dateOfBirth, GenderType gender, string? phoneNumber, IFormFile? image, string? personalCode, AddressDto? address)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Image = image;
            PersonalCode = personalCode;
            Address = address;
        }
    }


}
