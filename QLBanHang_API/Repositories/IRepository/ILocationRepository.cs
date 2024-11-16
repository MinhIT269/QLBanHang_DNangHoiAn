using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetListByNameAsync(string? name);
        Task<Location> GetLocationByIdAsync(Guid id);
        Task<Location> AddLocationAsync(Location location);
        Task<List<Location>> UpdateListLocationAsync(List<Location> location);
        Task<Location> UpdateLocationAsync(Location location);
        Task<Location> DeleteLocationAsync(Guid id);
    }
}
