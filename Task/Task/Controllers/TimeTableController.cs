using Microsoft.AspNetCore.Mvc;
using Task.Models;
using System.Collections.Generic;
using System.Linq;

namespace Task.Controllers
{
    public class TimetableController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TimetableInputModel());
        }

        [HttpPost]
        public IActionResult CalculateHours(TimetableInputModel model)
        {
            if (ModelState.IsValid)
            {
                model.TotalHours = model.WorkingDays * model.SubjectsPerDay;

                // Generate placeholder subjects
                for (int i = 1; i <= model.TotalSubjects; i++)
                {
                    model.Subjects.Add(new SubjectModel { Name = $"Subject {i}", Hours = 0 });
                }

                return View("SubjectHours", model);
            }
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult GenerateTimetable(TimetableInputModel model)
        {
            if (ModelState.IsValid && model.Subjects.Sum(s => s.Hours) == model.TotalHours)
            {
                // Call the renamed private method
                var timetable = CreateTimetable(model);

                ViewBag.Timetable = timetable;
                return View("Timetable", model);
            }

            ModelState.AddModelError("", "Total hours for all subjects must equal the weekly total.");
            return View("SubjectHours", model);
        }

        // Renamed private method
        private List<List<string>> CreateTimetable(TimetableInputModel model)
        {
            // Extract subject names based on their allocated hours
            var subjectQueue = new Queue<string>(
                model.Subjects.SelectMany(s => Enumerable.Repeat(s.Name, s.Hours))
            );

            var timetable = new List<List<string>>();

            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < model.WorkingDays; j++)
                {
                    if (subjectQueue.Any())
                    {
                        row.Add(subjectQueue.Dequeue());
                    }
                    else
                    {
                        row.Add(string.Empty); // Fill empty slots if necessary
                    }
                }
                timetable.Add(row);
            }

            return timetable;
        }
    }
}
