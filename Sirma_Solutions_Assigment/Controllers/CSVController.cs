using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sirma_Solutions_Assigment.BussinessLogic;
using Sirma_Solutions_Assigment.BussinessLogic.Contracts;
using Sirma_Solutions_Assigment.DTO.DTO;
using System.Text.Json;

namespace Sirma_Solutions_Assigment.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("csv")]
    public class CSVController : ControllerBase
   {
        private readonly ICSVService service;

        public CSVController(ICSVService service)
        {
            this.service = service;
        }

        [HttpPost("post")]
        public async Task<SharedWork> GetWorkingPairFromCSV([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return new SharedWork() { FirstEmployeeId = 400 };

            try
            {
                if (file.ContentType != Constants.FILE_TYPE_CSV)
                    return null;

                SharedWork result = await this.service.GetHardestWorkingPair(file.OpenReadStream());

                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
       
    }
}
