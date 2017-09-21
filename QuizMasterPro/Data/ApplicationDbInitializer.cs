using QuizMasterPro.Models.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using GenFu;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using QuizMasterPro.Models;
using System.Threading.Tasks;
using QuizMasterPro.Authorization;

namespace QuizMasterPro.Data
{
    public class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbInitializer(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Initialize()
        {
            _context.Database.EnsureCreated();

            await EnsureUsers();

            EnsureData();
        }

        private async Task EnsureUsers()
        {
            var aid = await EnsureUser("welcome123", "admin@contoso.com");
            await EnsureRole(aid, Constants.QuizAdministratorsRole);

            var uid = await EnsureUser("welcome123", "user@contoso.com");
            await EnsureRole(uid, Constants.QuizUsersRole);
        }

        private async Task<string> EnsureUser(string testUserPw, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userName, Email = userName };
                var result = await _userManager.CreateAsync(user, testUserPw);
            }
            return user.Id;
        }

        private async Task<IdentityResult> EnsureRole(string uid, string role)
        {
            IdentityResult identiyResult = null;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                identiyResult = await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await _userManager.FindByIdAsync(uid);
            identiyResult = await _userManager.AddToRoleAsync(user, role);

            return identiyResult;
        }

        private void EnsureData()
        {
            // Look for any students.
            if (_context.Quizzes.Any())
            {
                return;   // DB has been seeded
            }

            var count = 25;
            GenFu.GenFu.Configure<Quiz>()
                .Fill(x => x.Created).AsPastDate()
                .Fill(x => x.Description).AsLoremIpsumSentences(13);
            GenFu.GenFu.Configure<Question>()
                .Fill(x => x.Single).WithRandom(new List<bool> { true, false });
            GenFu.GenFu.Configure<Answer>()
                .Fill(x => x.Value).AsLoremIpsumWords(6);

            var quizes = A.ListOf<Quiz>(count).Select(quiz =>
            {
                quiz.Id = 0;
                quiz.Questions = new Collection<Question>(A.ListOf<Question>(15).Select(question =>
                {
                    question.Id = 0;
                    question.Answers = new Collection<Answer>(A.ListOf<Answer>(4).Select(answer =>
                    {
                        answer.Id = 0;
                        return answer;
                    }).ToList());
                    return question;
                }).ToList());
                return quiz;
            });
            _context.Quizzes.AddRange(quizes);
            _context.SaveChanges();
        }
    }
}
