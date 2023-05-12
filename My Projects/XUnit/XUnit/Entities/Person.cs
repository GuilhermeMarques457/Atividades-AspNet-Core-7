using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PersonGender { get; set; }
        public Guid? CountryID { get; set; }
        public string? PersonAddress { get; set; }
        public bool? ReceiveNewsLetters { get; set; }
    }
}
