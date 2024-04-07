using Data.DataAccess;
using Data.Enums;
using Data.Models;
using Data.ViewModels;
using Microsoft.AspNetCore.Http; // Import thư viện để sử dụng IFormFile
using MongoDB.Driver;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO; // Import thư viện để sử dụng MemoryStream
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IExcelService
    {
        ResponseModel ImportFromExcel(IFormFile file);
    }

    public class ExcelService : IExcelService
    {
        private readonly AppDbContext _dbContext;

        public ExcelService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseModel ImportFromExcel(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    var workbook = new XSSFWorkbook(stream);
                    var sheet = workbook.GetSheetAt(0);

                    var row2 = sheet.GetRow(1);
                    if (row2.GetCell(0)?.ToString()?.Trim() != "Mục" ||
                        row2.GetCell(1)?.ToString()?.Trim() != "Loại" ||
                        row2.GetCell(2)?.ToString()?.Trim() != "Tên phí" ||
                        row2.GetCell(3)?.ToString()?.Trim() != "Mã phí" ||
                        row2.GetCell(4)?.ToString()?.Trim() != "Tên vendor" ||
                        row2.GetCell(5)?.ToString()?.Trim() != "Hãng tàu/đại lý" ||
                        row2.GetCell(6)?.ToString()?.Trim() != "Loại hàng" ||
                        row2.GetCell(7)?.ToString()?.Trim() != "Loại cont" ||
                        row2.GetCell(8)?.ToString()?.Trim() != "Cảng xếp" ||
                        row2.GetCell(9)?.ToString()?.Trim() != "Cảng dỡ" ||
                        row2.GetCell(10)?.ToString()?.Trim() != "Tên kho" ||
                        row2.GetCell(11)?.ToString()?.Trim() != "Điểm đến/nhà máy" ||
                        row2.GetCell(12)?.ToString()?.Trim() != "Loại phương tiện" ||
                        row2.GetCell(13)?.ToString()?.Trim() != "Phương thức giao nhận" ||
                        row2.GetCell(14)?.ToString()?.Trim() != "Tác nghiệp" ||
                        row2.GetCell(15)?.ToString()?.Trim() != "Mặt hàng" ||
                        row2.GetCell(16)?.ToString()?.Trim() != "Đơn vị tính" ||
                        row2.GetCell(17)?.ToString()?.Trim() != "Đơn giá")
                    {
                        throw new Exception("File không đúng định dạng. Các trường dữ liệu từ ô A2 -> R2 phải theo đúng thứ tự: " +
                            "Mục - Loại - Tên phí - Mã phí - Tên vendor - Hãng tàu/đại lý - Loại hàng - Loại cont - Cảng xếp - Cảng dỡ - Tên kho - " +
                            "Điểm đến/nhà máy - Loại phương tiện - Phương thức giao nhận - Tác nghiệp - Mặt hàng - Đơn vị tính - Đơn giá");
                    }

                    var cangNhapKhauList = new List<CangNhapKhau>();
                    var khaiThacCangList = new List<KhaiThacCang>();

                    for (int i = (sheet.FirstRowNum + 2); i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null) continue;

                        var loai = row.GetCell(1)?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(loai))
                        {
                            if (loai == "1. Cảng nhập khẩu")
                            {
                                var donGiaString = row.GetCell(17)?.ToString()?.Trim();
                                double donGia;

                                if (double.TryParse(donGiaString, out donGia))
                                {
                                    var cangNhapKhau = new CangNhapKhau
                                    {
                                        TenPhi = row.GetCell(2)?.ToString()?.Trim(),
                                        MaPhi = row.GetCell(3)?.ToString()?.Trim(),
                                        TenVendor = row.GetCell(4)?.ToString()?.Trim(),
                                        HangTau = row.GetCell(5)?.ToString()?.Trim(),
                                        LoaiHang = Enum.Parse<LoaiHang>(row.GetCell(6)?.ToString()?.Trim()),
                                        LoaiCont = row.GetCell(7)?.ToString()?.Trim(),
                                        DonViTinh = row.GetCell(16)?.ToString()?.Trim(),
                                        DonGia = donGia
                                    };
                                    cangNhapKhauList.Add(cangNhapKhau);
                                }
                            }
                            else if (loai == "2. Khai thác cảng")
                            {
                                var donGiaString = row.GetCell(17)?.ToString()?.Trim();
                                double donGia;
                                if (double.TryParse(donGiaString, out donGia))
                                {
                                    var khaiThacCang = new KhaiThacCang
                                    {
                                        TenPhi = row.GetCell(2)?.ToString()?.Trim(),
                                        MaPhi = row.GetCell(3)?.ToString()?.Trim(),
                                        TenVendor = row.GetCell(4)?.ToString()?.Trim(),
                                        LoaiHang = Enum.Parse<LoaiHang>(row.GetCell(6)?.ToString()?.Trim()),
                                        LoaiCont = row.GetCell(7)?.ToString()?.Trim(),
                                        CangXep = row.GetCell(8)?.ToString()?.Trim(),
                                        CangDo = row.GetCell(9)?.ToString()?.Trim(),
                                        LoaiPhuongTien = row.GetCell(12)?.ToString()?.Trim(),
                                        PhuongThucGiaoNhan = row.GetCell(13)?.ToString()?.Trim(),
                                        DonViTinh = row.GetCell(16)?.ToString()?.Trim(),
                                        DonGia = donGia
                                    };
                                    khaiThacCangList.Add(khaiThacCang);
                                }
                            }
                        }
                    }

                    if (!CheckForDuplicates(cangNhapKhauList, khaiThacCangList))
                    {
                        _dbContext.CangNhapKhau.InsertMany(cangNhapKhauList);
                        _dbContext.KhaiThacCang.InsertMany(khaiThacCangList);
                    }
                    else throw new Exception("Data bị trùng lặp. Vui lòng loại bỏ những dữ liệu đã tồn tại trước khi import vào CSDL");

                    return new ResponseModel { Succeeded = true, Message = "Import Successfully" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseModel { Succeeded = false, Message = ex.Message };
            }
        }

        private bool CheckForDuplicates(List<CangNhapKhau> cangNhapKhauList, List<KhaiThacCang> khaiThacCangList)
        {
            var existingCangNhapKhau = _dbContext.CangNhapKhau.Find(_ => true).ToList();
            var existingKhaiThacCang = _dbContext.KhaiThacCang.Find(_ => true).ToList();

            // Kiểm tra trùng lặp trong danh sách CangNhapKhau
            foreach (var newCangNhapKhau in cangNhapKhauList)
            {
                if (existingCangNhapKhau.Any(existing => AreRowsEqualInCangNhapKhau(newCangNhapKhau, existing)))
                    return true;
            }

            // Kiểm tra trùng lặp trong danh sách KhaiThacCang
            foreach (var newKhaiThacCang in khaiThacCangList)
            {
                if (existingKhaiThacCang.Any(existing => AreRowsEqualInKhaiThacCang(newKhaiThacCang, existing)))
                    return true;
            }

            return false;
        }
        private bool AreRowsEqualInCangNhapKhau(CangNhapKhau newRow, CangNhapKhau existingRow)
        {
            return newRow.TenPhi == existingRow.TenPhi &&
                   newRow.MaPhi == existingRow.MaPhi &&
                   newRow.TenVendor == existingRow.TenVendor &&
                   newRow.HangTau == existingRow.HangTau &&
                   newRow.LoaiHang == existingRow.LoaiHang &&
                   newRow.LoaiCont == existingRow.LoaiCont &&
                   newRow.DonViTinh == existingRow.DonViTinh &&
                   newRow.DonGia == existingRow.DonGia;
        }
        private bool AreRowsEqualInKhaiThacCang(KhaiThacCang newRow, KhaiThacCang existingRow)
        {
            return newRow.TenPhi == existingRow.TenPhi &&
                   newRow.MaPhi == existingRow.MaPhi &&
                   newRow.TenVendor == existingRow.TenVendor &&
                   newRow.LoaiHang == existingRow.LoaiHang &&
                   newRow.LoaiCont == existingRow.LoaiCont &&
                   newRow.CangXep == existingRow.CangXep &&
                   newRow.CangDo == existingRow.CangDo &&
                   newRow.LoaiPhuongTien == existingRow.LoaiPhuongTien &&
                   newRow.PhuongThucGiaoNhan == existingRow.PhuongThucGiaoNhan &&
                   newRow.DonViTinh == existingRow.DonViTinh &&
                   newRow.DonGia == existingRow.DonGia;
        }
    }
}
