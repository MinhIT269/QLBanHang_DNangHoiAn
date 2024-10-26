using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Services.IService
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetListByName(string name);
        Task<LocationDto> GetLocationById(Guid id);
        Task<LocationDto> AddLocation(AddUpLocationDto location);
        Task<LocationDto> UpdateLocation(Guid id,AddUpLocationDto upLocation);
        Task<LocationDto> DeleteLocation(Guid id);
    }
}
