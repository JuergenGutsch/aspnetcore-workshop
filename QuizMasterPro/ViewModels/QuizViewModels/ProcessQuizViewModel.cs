using System;
using System.Collections.Generic;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    public class ProcessQuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<ProcessQuestionViewModel> Questions { get; set; }
    }

    public class ProcessQuestionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Single { get; set; }
        public IEnumerable<ProcessAnswerViewModel> Answers { get; set; }
    }

    public class ProcessAnswerViewModel
    {
        public int Id { get; set; }       
        public string Value { get; set; }
    }
}
