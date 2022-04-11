using Microsoft.AspNetCore.Mvc;
using Sirma_Solutions_Assigment.BussinessLogic;
using Sirma_Solutions_Assigment.BussinessLogic.Contracts;
using Sirma_Solutions_Assigment.DTO.DTO;
using Sirma_Solutions_Assigment.Web.ViewModels;

namespace Sirma_Solutions_Assigment.Web.Controllers
{
    [Route("csv")]
    public class CSVController : Controller
    {
        private readonly ICSVService csvService;

        public CSVController(ICSVService csvService)
        {
            this.csvService = csvService;
        }

        [HttpPost("pair")]
        public async Task<IActionResult> GetHardestWorkingPair([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                if (file.ContentType != Constants.FILE_TYPE_CSV)
                    return BadRequest();

                SharedWork result = await this.csvService.GetHardestWorkingPair(file.OpenReadStream());
                    
                DataViewModel model = new DataViewModel()
                {
                    EmployeeId1 = result.FirstEmployeeId,
                    EmployeeId2 = result.SecondEmployeeId,
                    ProjectsAndWorktime = result.ProjectIdsAndWorkTime
                };

                return View("Data", model);

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
