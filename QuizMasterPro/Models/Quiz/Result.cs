using System;
using System.ComponentModel.DataAnnotations;

namespace QuizMasterPro.Models.Quiz
{
    public class Result
    {
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string UserId { get; set; }
        [Required]
        public DateTime DoneAt { get; set; }
    }
}
