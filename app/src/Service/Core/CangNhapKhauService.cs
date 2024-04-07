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
    public interface ICangNhapKhauService
    {
        ResponseModel GetByFilter(CangNhapKhauFilter cangNhapKhauFilter);
        ResponseModel GetHangTau();
    }
    public class CangNhapKhauService : ICangNhapKhauService
    {
        private readonly AppDbContext _dbContext;

        public CangNhapKhauService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseModel GetByFilter(CangNhapKhauFilter cangNhapKhauFilter)
        {
            try
            {
                var filterBuilder = Builders<CangNhapKhau>.Filter;
                var filterDefinition = filterBuilder.Empty;

                if (!string.IsNullOrEmpty(cangNhapKhauFilter.HangTau))
                    filterDefinition &= filterBuilder.Eq(x => x.HangTau, cangNhapKhauFilter.HangTau);

                if (cangNhapKhauFilter.LoaiHang.HasValue)
                    filterDefinition &= filterBuilder.Eq(x => x.LoaiHang, cangNhapKhauFilter.LoaiHang);

                if (!string.IsNullOrEmpty(cangNhapKhauFilter.LoaiCont))
                    filterDefinition &= filterBuilder.Eq(x => x.LoaiCont, cangNhapKhauFilter.LoaiCont);

                var result = _dbContext.CangNhapKhau.Find(filterDefinition).ToList();

                if (result.Count < 1)
                    throw new Exception("No Data");

                var response = result.Adapt<List<CangNhapKhauDto>>();

                return new ResponseModel { Data = response, Succeeded = true, Message = "Got Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Succeeded = true, Message = ex.Message };
            }
        }

        public ResponseModel GetHangTau()
        {
            var distinctHangTau = _dbContext.CangNhapKhau.Distinct<string>("HangTau", new BsonDocument()).ToList();

            return new ResponseModel { Data = distinctHangTau, Succeeded = true, Message = "Got Successfully" };
        }
    }
}