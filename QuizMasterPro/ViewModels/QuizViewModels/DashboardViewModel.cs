using QuizMasterPro.Models.Quiz;
using System.Collections.Generic;
using System;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<DashboardQuizItemViewModel> LatestQuizzes { get; set; }
        public IEnumerable<DashboardQuizItemViewModel> MyQuizzes { get; set; }
    }

    public class DashboardQuizItemViewModel
    {
        public int Id { get; internal set; }
        public string Title { get; internal set; }
        public DateTime Created { get; internal set; }
    }
}
