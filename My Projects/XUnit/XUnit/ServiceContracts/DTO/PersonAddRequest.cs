﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Person Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        public string? PersonEmail { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? PersonGender { get; set; }
        public Guid? CountryID { get; set; }
        public string? PersonAddress { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object to Person object
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person() 
            {
                PersonName = this.PersonName,
                PersonEmail = this.PersonEmail,
                DateOfBirth = this.DateOfBirth,
                PersonGender = this.PersonGender.ToString(),
                CountryID = this.CountryID,
                PersonAddress = this.PersonAddress,
                ReceiveNewsLetters = this.ReceiveNewsLetters,
            };
        }
    }
}
