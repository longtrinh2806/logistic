using Data.DataAccess;
using Data.Dtos;
using Data.Filters;
using Data.Models;
using Data.ViewModels;
using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IKhaiThacCangService
    {
        ResponseModel GetByFilter(KhaiThacCangFilter khaiThacCangFilter);
        ResponseModel GetCangDo();
        ResponseModel GetDonViKhaiThac();
        ResponseModel GetPhuongThucGiaoNhan();
        ResponseModel GetPhuongTien();
    }
    public class KhaiThacCangService : IKhaiThacCangService
    {
        private readonly AppDbContext _dbContext;

        public KhaiThacCangService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseModel GetByFilter(KhaiThacCangFilter khaiThacCangFilter)
        {
            try
            {
                var filterBuilder = Builders<KhaiThacCang>.Filter;
                var filterDefinition = filterBuilder.Empty;

                if (!string.IsNullOrEmpty(khaiThacCangFilter.CangDo))
                    filterDefinition &= filterBuilder.Eq(x => x.CangDo, khaiThacCangFilter.CangDo);

                if (!string.IsNullOrEmpty(khaiThacCangFilter.DonViKhaiThac))
                    filterDefinition &= filterBuilder.Eq(x => x.TenVendor, khaiThacCangFilter.DonViKhaiThac);

                if (!string.IsNullOrEmpty(khaiThacCangFilter.PhuongTienNhanHang))
                    filterDefinition &= filterBuilder.Eq(x => x.LoaiPhuongTien, khaiThacCangFilter.PhuongTienNhanHang);

                if (!string.IsNullOrEmpty(khaiThacCangFilter.PhuongThucGiaoNhan))
                    filterDefinition &= filterBuilder.Eq(x => x.PhuongThucGiaoNhan, khaiThacCangFilter.PhuongThucGiaoNhan);

                var result = _dbContext.KhaiThacCang.Find(filterDefinition).ToList();

                if (result.Count < 1)
                    throw new Exception("No Data");

                var response = result.Adapt<List<KhaiThacCangDto>>();

                return new ResponseModel { Data = response, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }

        public ResponseModel GetCangDo()
        {
            try
            {
                var distinctCangDo = _dbContext.KhaiThacCang.Distinct<string>("CangDo", new BsonDocument()).ToList();

                if (distinctCangDo.Count < 1)
                    throw new Exception("No Data");

                return new ResponseModel { Data = distinctCangDo, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }

        public ResponseModel GetDonViKhaiThac()
        {
            try
            {
                var distinctDVKT = _dbContext.KhaiThacCang.Distinct<string>("TenVendor", new BsonDocument()).ToList();

                if (distinctDVKT.Count < 1)
                    throw new Exception("No Data");

                return new ResponseModel { Data = distinctDVKT, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }

        public ResponseModel GetPhuongThucGiaoNhan()
        {
            try
            {
                var distinctPhuongThucGiaoNhan = _dbContext.KhaiThacCang.Distinct<string>("PhuongThucGiaoNhan", new BsonDocument()).ToList();

                if (distinctPhuongThucGiaoNhan.Count < 1)
                    throw new Exception("No Data");

                return new ResponseModel { Data = distinctPhuongThucGiaoNhan, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }

        public ResponseModel GetPhuongTien()
        {
            try
            {
                var distinctPhuongTien = _dbContext.KhaiThacCang.Distinct<string>("LoaiPhuongTien", new BsonDocument()).ToList();

                if (distinctPhuongTien.Count < 1)
                    throw new Exception("No Data");

                return new ResponseModel { Data = distinctPhuongTien, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }
    }
}
