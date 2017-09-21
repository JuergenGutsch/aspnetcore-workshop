using Microsoft.AspNetCore.Mvc;
using QuizMasterPro.Models.Quiz;
using QuizMasterPro.ViewModels.ModelBinders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuizMasterPro.ViewModels.QuizViewModels
{
    [ModelBinder(BinderType = typeof(ResultViewModelBinder))]
    public class ResultViewModel
    {
        public ICollection<Result> Results { get; set; } = new Collection<Result>();
    }
}
