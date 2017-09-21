using QuizMasterPro.Models.Quiz;
using System;
using System.Collections.Generic;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    public class QuizesListViewModel
    {
        public IEnumerable<QuizesListItemViewModel> Quizzes { get; set; }
    }

    public class QuizesListItemViewModel
    {
        public int Id { get; internal set; }
        public string Title { get; internal set; }
        public DateTime Created { get; internal set; }
    }
}
