using PBL6.Dto.Request;
using PBL6.Dto;
using QLBanHang_API.Dto;

namespace PBL6.Services.IService
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetListByName(string name);
        Task<LocationDto> GetLocationById(Guid id);
        Task<LocationDto> AddLocation(LocationRequest location);
        Task<LocationDto> UpdateLocation(Guid id, UpLocationDto upLocation);
        Task<LocationDto> DeleteLocation(Guid id);
    }
}
