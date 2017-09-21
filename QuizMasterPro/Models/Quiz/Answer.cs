using System.ComponentModel.DataAnnotations;

namespace QuizMasterPro.Models.Quiz
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public bool IsAnswer { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
