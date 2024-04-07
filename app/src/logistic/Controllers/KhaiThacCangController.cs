using Data.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace logistic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhaiThacCangController : ControllerBase
    {
        private readonly IKhaiThacCangService _khaiThacCangService;

        public KhaiThacCangController(IKhaiThacCangService khaiThacCangService)
        {
            _khaiThacCangService = khaiThacCangService;
        }

        [HttpGet]
        public IActionResult GetByFilter([FromQuery] KhaiThacCangFilter khaiThacCangFilter)
        {
            var result = _khaiThacCangService.GetByFilter(khaiThacCangFilter);

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }

        [HttpGet("cang-do")]
        public IActionResult GetCangDo()
        {
            var result = _khaiThacCangService.GetCangDo();

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }

        [HttpGet("don-vi-khai-thac")]
        public IActionResult GetDonViKhaiThac()
        {
            var result = _khaiThacCangService.GetDonViKhaiThac();

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }

        [HttpGet("phuong-tien")]
        public IActionResult GetPhuongTien()
        {
            var result = _khaiThacCangService.GetPhuongTien();

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }

        [HttpGet("phuong-thuc-giao-nhan")]
        public IActionResult GetPhuongThucGiaoNhan()
        {
            var result = _khaiThacCangService.GetPhuongThucGiaoNhan();

            if (result.Succeeded)
                return Ok(result);
            return StatusCode(400, result);
        }
        
    }
}
