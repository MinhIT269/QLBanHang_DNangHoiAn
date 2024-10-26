using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
	public interface IBrandRepository
	{
		Task<List<Brand>> GetAllBrandsAsync();
	}
}
