using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizMasterPro.Models.Quiz
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuizId { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Single { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
