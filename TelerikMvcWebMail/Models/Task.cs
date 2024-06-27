namespace TelerikMvcWebMail.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Task
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime? DateCreated { get; set; }
        public string NoteContent { get; set; }
        public string Category { get; set; }
    }
}
