using ConferenceManagement.Models;
using ConferenceManagement.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConferenceManagement.Controllers
{
    public class HomeController : Controller
    {
        private const string InvalidFileMessage = "Please submit a .txt file";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Schedule(HttpPostedFileBase inputFile)
        {
            // Invalid file
            if (!IsValidFile(inputFile, out string errorMessage))
            {
                ViewBag.Message = errorMessage;
                return View("Index");
            }

            var lineParser = new InputLineParser();
            var inputDataParser = new ConferenceInputDataParser(lineParser);
            var talks =  await inputDataParser.LoadTalks(inputFile);

            if (talks.Count == 0)
            {
                ViewBag.Message = "No valid registered talks found in the input file.";
            }

            var tracks = inputDataParser.GetTracks(talks);
            var recursiveScheduler = new NonRecursiveScheduler();
            var scheduleManager = new ScheduleManager(recursiveScheduler);
            scheduleManager.Schedule(talks, tracks);

            var output = new ScheduledResult
            {
                Tracks = tracks
            };
            OutputSet.Items.Add(output);
            return RedirectToAction("ScheduleDetails", new { id = output.Id });
        }

        private bool IsValidFile(HttpPostedFileBase inputFile, out string errorMessage)
        {
            bool isValid = true;
            errorMessage = null;

            if (inputFile == null)
            {
                isValid = false;
                errorMessage = "Please submit a file with Conference talk details.";
                return isValid;
            }

            if (!string.Equals(inputFile.ContentType, "text/plain", 
                StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = false;
                errorMessage = InvalidFileMessage;
                return isValid;
            }

            var fileExtension = Path.GetExtension(inputFile.FileName);

            if (fileExtension == null ||
                !string.Equals(fileExtension, ".txt", StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = false;
                errorMessage = InvalidFileMessage;
            }

            return isValid;
        }
        

        [HttpGet]
        public ActionResult ScheduleDetails(Guid id)
        {
            if (id == Guid.Empty)
            {
                return View("Error");
            }

            var scheduleItem = OutputSet.Items.FirstOrDefault(i => i.Id == id);

            if (scheduleItem == null)
            {
                return View("Error");
            }

            return View(scheduleItem);
        }

    }
}