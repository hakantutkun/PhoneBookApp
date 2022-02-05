namespace PersonService.Core.Models
{
    /// <summary>
    /// Person Model
    /// </summary>
    public class Person
    {
        /// <summary>
        /// UID of Person
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Name of Person
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Surname of Person
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Company of Person
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Person Information
        /// </summary>
        public List<ContactInfo>? ContactInfo { get; set; }
    }
}
