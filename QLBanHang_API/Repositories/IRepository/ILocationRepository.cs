using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
	public interface ILocationRepository
	{
		Task<List<Location>> GetListByNameAsync(string? name);
		Task<Location> GetLocationByIdAsync(Guid id);
		Task<Location> AddLocationAsync(Location location);
		Task<Location> UpdateLocationAsync(Guid id, Location location);
		Task<Location> DeleteLocationAsync(Guid id);
	}
}
