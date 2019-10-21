using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BudgetController : Controller
    {
        private readonly DefaultDbContext _context;

        public BudgetController(DefaultDbContext context)
        {
          _context = context;
        }

        [HttpGet("")]
        public IEnumerable<Budget> Get(int? year = null,
                                       int? month = null,
                                       Department? department = null,
                                       int? startMonth = null,
                                       int? startYear = null,
                                       int? endMonth = null,
                                       int? endYear = null)
        {
            IQueryable<Budget> query = _context.Budgets;

            if (year.HasValue)
            {
                query = query.Where(x => x.CalendarYear == year.Value);
            }

            if (month.HasValue)
            {
                query = query.Where(x => x.CalendarMonth == month.Value);
            }

            if (department.HasValue)
            {
                query = query.Where(x => x.Department == department.Value);
            }

            if (startYear.HasValue && !startMonth.HasValue)
            {
                query = query.Where(x => x.CalendarYear >= startYear.Value);
            }

            if (endYear.HasValue && !endMonth.HasValue)
            {
                query = query.Where(x => x.CalendarYear <= endYear.Value);
            }

            if (startYear.HasValue && startMonth.HasValue)
            {
                query = query.Where(x => (x.CalendarYear > startYear.Value) || (x.CalendarYear == startYear.Value && x.CalendarMonth >= startMonth.Value));
            }

            if (endYear.HasValue && endMonth.HasValue)
            {
                query = query.Where(x => (x.CalendarYear < endYear.Value) || (x.CalendarYear == endYear.Value && x.CalendarMonth <= endMonth.Value));
            }

            return query.OrderBy(o => o.CalendarYear).ThenBy(o => o.CalendarMonth);
        }

        [HttpGet("export")]
        public IActionResult Export(int? year = null,
                                    int? month = null,
                                    Department? department = null,
                                    int? startMonth = null,
                                    int? startYear = null,
                                    int? endMonth = null,
                                    int? endYear = null)
        {
            var csv = Get(year, month, department, startMonth, startYear, endMonth, endYear).ToCsv();

            var buffer = Encoding.ASCII.GetBytes(csv);

            Response.Headers.Add("Content-Disposition", "attachment");

            return File(buffer, "application/vnd.ms-excel", $"BudgetExport-{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv");
        }
    }
}
