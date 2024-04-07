using Data.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class KhaiThacCang
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; }
        public string TenPhi { get; set; }
        public string MaPhi { get; set; }
        public string TenVendor { get; set; }
        public LoaiHang LoaiHang { get; set; }
        public string? LoaiCont { get; set; }
        public string CangXep { get; set; }
        public string CangDo { get; set; }
        public string LoaiPhuongTien { get; set; }
        public string PhuongThucGiaoNhan { get; set; }
        public string DonViTinh { get; set; }
        public double DonGia { get; set; }
    }
}
