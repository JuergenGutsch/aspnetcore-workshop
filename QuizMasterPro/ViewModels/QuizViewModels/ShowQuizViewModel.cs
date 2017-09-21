using System;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    public class ShowQuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int QuestionsCount { get; set; }
    }
}
