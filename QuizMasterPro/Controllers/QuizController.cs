using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMasterPro.Data;
using QuizMasterPro.ViewModels.QuizViewModels;
using QuizMasterPro.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using QuizMasterPro.Models;
using System.Threading.Tasks;

namespace QuizMasterPro.Controllers
{
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;


        public QuizController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var quizzes = _dbContext.Quizzes
                .OrderByDescending(x => x.Created)
                .Select(x => new QuizesListItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Created = x.Created
                });
            var model = new QuizesListViewModel
            {
                Quizzes = quizzes
            };
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Show([FromRoute]int id)
        {
            var quiz = _dbContext.Quizzes
                .Include(x => x.Questions)
                .FirstOrDefault(x => x.Id == id);
            var model = new ShowQuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Created = quiz.Created,
                QuestionsCount = quiz.Questions.Count()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Process([FromRoute] int id)
        {
            var quiz = _dbContext.Quizzes
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == id);
            var model = new ProcessQuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Created = quiz.Created,
                Questions = quiz.Questions.Select(question => new ProcessQuestionViewModel
                {
                    Id = question.Id,
                    Title = question.Title,
                    Single = question.Single,
                    Answers = question.Answers.Select(answer => new ProcessAnswerViewModel
                    {
                        Id = answer.Id,
                        Value = answer.Value
                    })
                })
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Answer([FromRoute] int id, ResultViewModel result)
        {
            if (!ModelState.IsValid)
            {
                return View("Process");
            }
            
            _dbContext.Results.AddRange(result.Results);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "QuizAdministrators")]
        public IActionResult Edit(int id)
        {
            var quiz = _dbContext.Quizzes
                .Include(x => x.Questions)
                .FirstOrDefault(x => x.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            var model = new EditQuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "QuizAdministrators")]
        public IActionResult Edit(int id, EditQuizViewModel model)
        {
            var quiz = _dbContext.Quizzes
                   .Include(x => x.Questions)
                   .FirstOrDefault(x => x.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                quiz.Title = model.Title;
                quiz.Description = model.Description;

                _dbContext.Update(quiz);
                _dbContext.SaveChanges();

                return RedirectToAction("Show", new { id = id });
            }


            return View(model);
        }
    }
}