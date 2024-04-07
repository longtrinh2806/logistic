using Data.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace logistic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CangNhapKhauController : ControllerBase
    {
        private readonly ICangNhapKhauService _cangNhapKhauService;

        public CangNhapKhauController(ICangNhapKhauService cangNhapKhauService)
        {
            _cangNhapKhauService = cangNhapKhauService;
        }

        [HttpGet]
        public IActionResult GetByFilter([FromQuery] CangNhapKhauFilter cangNhapKhauFilter)
        {
            var result = _cangNhapKhauService.GetByFilter(cangNhapKhauFilter);

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }

        [HttpGet("hang-tau")]
        public IActionResult GetHangTau()
        {
            var result = _cangNhapKhauService.GetHangTau();

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }
    }
}
