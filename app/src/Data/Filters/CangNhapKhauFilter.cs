using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Filters
{
    public class CangNhapKhauFilter
    {
        public string HangTau { get; set; }
        public LoaiHang? LoaiHang { get; set; }
        public string? LoaiCont { get; set; }
    }
}
