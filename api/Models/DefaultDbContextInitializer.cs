using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class DefaultDbContextInitializer : IDefaultDbContextInitializer
    {
        private readonly DefaultDbContext _context;
        private readonly UserManager<Contributor> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DefaultDbContextInitializer(DefaultDbContext context, UserManager<Contributor> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public bool EnsureCreated()
        {
            return _context.Database.EnsureCreated();
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        public async Task Seed()
        {
            //Seed test contributors
            for (var i = 1; i <= 5; i++)
            {
                var email = $"user{i}@test.com";

                if (await _userManager.FindByEmailAsync(email) == null)
                {
                  var user = new Contributor
                  {
                    UserName = $"testuser{i}",
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = $"User{i}"
                  };

                  await _userManager.CreateAsync(user, $"P@ssword{i}");
                }
            }

            if (!_context.Budgets.Any())
            {
                var rand = new Random();

                for (var i = 1; i <= 12; i++)
                {
                    _context.Budgets.Add(new Budget() { CalendarYear = 2016, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Marketing });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2016, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Products });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2016, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Sales });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2016, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Services });

                    _context.Budgets.Add(new Budget() { CalendarYear = 2017, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Marketing });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2017, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Products });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2017, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Sales });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2017, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Services });

                    _context.Budgets.Add(new Budget() { CalendarYear = 2018, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Marketing });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2018, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Products });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2018, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Sales });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2018, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = rand.Next(2000, 22000), Department = Department.Services });

                    _context.Budgets.Add(new Budget() { CalendarYear = 2019, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = i >= DateTime.Now.Month ? 0 : rand.Next(2000), Department = Department.Marketing });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2019, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = i >= DateTime.Now.Month ? 0 : rand.Next(2000), Department = Department.Products });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2019, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = i >= DateTime.Now.Month ? 0 : rand.Next(2000), Department = Department.Sales });
                    _context.Budgets.Add(new Budget() { CalendarYear = 2019, CalendarMonth = i, AmountBudgeted = rand.Next(5000, 20000), AmountSpent = i >= DateTime.Now.Month ? 0 : rand.Next(2000), Department = Department.Services });
                }
                
                _context.SaveChanges();
            }
        }
    }

    public interface IDefaultDbContextInitializer
    {
        bool EnsureCreated();
        void Migrate();
        Task Seed();
    }
}
