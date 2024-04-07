using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace logistic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost]
        public IActionResult ImportFromExcel([FromForm] IFormFile file)
        {
            var result = _excelService.ImportFromExcel(file);

            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
