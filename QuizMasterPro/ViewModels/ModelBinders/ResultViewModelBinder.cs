using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuizMasterPro.Data;
using QuizMasterPro.Models.Quiz;
using QuizMasterPro.ViewModels.QuizViewModels;
using System;
using System.Threading.Tasks;

namespace QuizMasterPro.ViewModels.ModelBinders
{
    public class ResultViewModelBinder : IModelBinder
    {
        private readonly ApplicationDbContext _db;
        public ResultViewModelBinder(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {            
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Specify a default argument name if none is set by ModelBinderAttribute
            var modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = "id";
            }

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;
            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            int id = 0;
            if (!int.TryParse(value, out id))
            {
                // Non-integer arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Quiz Id must be an integer.");
                return Task.CompletedTask;
            }

            var model = new ResultViewModel();
            var values = bindingContext.HttpContext.Request.Form;
            foreach (var item in values)
            {
                if (!item.Key.StartsWith("answer-"))
                {
                    continue;
                }
                var questionId = Convert.ToInt32(item.Key.Replace("answer-", String.Empty));
                foreach(var val in item.Value)
                {
                    var answerId = Convert.ToInt32(val);
                    model.Results.Add(new Result
                    {
                        QuizId = id,
                        DoneAt = DateTime.Now,
                        QuestionId = questionId,
                        AnswerId = answerId,
                        UserId = "juergen@gutsch-online.de"
                    });
                }
            }

            // Model will be null if not found, including for 
            // out of range id values (0, -3, etc.)
            //var model = _db.Authors.FirstOrDefault(x => x.Id == id);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }

}
