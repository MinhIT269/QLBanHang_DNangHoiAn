using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;

namespace QLBanHang_API.Services.IService
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetListByName(string name);
        Task<LocationDto> GetLocationById(Guid id);
        Task<LocationDto> AddLocation(LocationRequest location);
        Task<LocationDto> UpdateLocation(Guid id,UpLocationDto upLocation);
        Task<LocationDto> DeleteLocation(Guid id);
    }
}
