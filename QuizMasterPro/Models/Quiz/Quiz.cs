using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizMasterPro.Models.Quiz
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
