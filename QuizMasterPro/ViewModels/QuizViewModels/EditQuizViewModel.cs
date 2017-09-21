using System.ComponentModel.DataAnnotations;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    public class EditQuizViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
