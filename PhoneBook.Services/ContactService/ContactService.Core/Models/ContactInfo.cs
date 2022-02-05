using PersonService.Core.Models;

namespace ContactService.Core.Models
{
    /// <summary>
    /// Contact Info Model
    /// </summary>
    public class ContactInfo
    {
        /// <summary>
        /// Phone Number of contact
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// A mail adress of contact
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Location info of contact
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Person Id Information
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Person Information
        /// </summary>
        public Person? Person { get; set; }
    }
}
