using FitnessHere.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessHere.Controllers
{
    public class ImportController : Controller
    {
        private readonly AdoNetImport _importRepository;

        public ImportController(AdoNetImport importRepository)
        {
            _importRepository = importRepository;
        }
        

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile importFile)
        {
            try
            {
                if (importFile == null || importFile.Length == 0)
                {
                    ViewBag.Message = "No file uploaded.";
                    return View("Index");
                }

                string fileExtension = Path.GetExtension(importFile.FileName).ToLower();

                if (fileExtension == ".xml")
                {
                    string xmlData;
                    using (var stream = new StreamReader(importFile.OpenReadStream()))
                    {
                        xmlData = stream.ReadToEnd(); // Read XML content as a string
                    }

                    bool isSuccess = _importRepository.ImportXmlToDatabase(xmlData);

                    ViewBag.Message = isSuccess ? "XML file imported successfully." : "Failed to import XML file.";
                }
                else if (fileExtension == ".json")
                {
                    string xmlData;
                    using (var stream = new StreamReader(importFile.OpenReadStream()))
                    {
                        xmlData = stream.ReadToEnd(); // Read XML content as a string
                    }

                    bool isSuccess = _importRepository.ImportJsonToDatabase(xmlData);

                    ViewBag.Message = isSuccess ? "JSON file imported successfully." : "Failed to import JSON file.";
                }
                else
                {
                    ViewBag.Message = "Invalid file type. Please upload an XML or JSON file.";
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while processing the file.";
                return View("Index");
            }
        }
    }
}
