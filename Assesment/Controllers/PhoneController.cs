using Assesment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Assesment.Controllers
{
    public class PhoneController : Controller
    {
        private readonly ILogger<HomeController> _logger; 
        public PhoneController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(InputModel model)
        {
            if (ModelState.IsValid)
            {
                List<string> outputNumbers  = ProcessNumbers(model.InputText);
                model.DetectedNumbers = outputNumbers;

                //if (model.DetectedNumbers.Count > 0)
                //{
                //    ModelState.AddModelError("InputText", "Phone numbers not detected!");
                //}
            }
            return View(model);
        }
        public string FormatPhoneNumber(string input)
        {
            string numericString = input;
            try
            {
                Dictionary<string, string> wordToDigit = new Dictionary<string, string>()
            {
                { "zero", "0" }, { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" },
                { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" },
                { "ten", "10" }, { "0", "0" }, { "1", "1" }, { "2", "2" }, { "3", "3" }, { "4", "4" },
                { "5", "5" }, { "6", "6" }, { "7", "7" }, { "8", "8" }, { "9", "9" },
                { "एक", "1" }, { "दो", "2" }, { "तीन", "3" }, { "चार", "4" },
                { "पाँच", "5" }, { "छह", "6" }, { "सात", "7" }, { "आठ", "8" }, { "नौ", "9" },
                { "शून्य", "0" }
            };
                // Replace words with corresponding digits
                foreach (var kvp in wordToDigit)
                {
                    input = Regex.Replace(input, kvp.Key, kvp.Value, RegexOptions.IgnoreCase);
                }
                numericString = Regex.Replace(input, @"\D", ""); // remove non digits
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in FormatPhoneNumber method:", ex.Message.ToString());
            }

            return numericString;
        }
        public List<string> ProcessNumbers(string input)
        {
            List<string> results = new List<string>();
            try
            {
                string[] phoneNumbers = input.Split(new[] { " or " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var number in phoneNumbers)
                {                     
                    string cleanedNumber = FormatPhoneNumber(number.Trim());
                    if (cleanedNumber != "")
                    {
                        if (cleanedNumber.Length == 10)
                        {
                            results.Add($"Number detected: {cleanedNumber}");
                        }
                        else
                        {
                            results.Add("Number not correct");
                        }
                    }
                    else { results.Add("Input number not correct"); }
                }
            } catch (Exception ex) 
            {
                _logger.LogError("Error occured in ProcessNumbers method:", ex.Message.ToString());
            }
            return results;
        }    
    
    }
}
