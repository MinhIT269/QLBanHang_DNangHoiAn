using AutoMapper;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;

namespace QLBanHang_API.Service
{
	public class BrandService : IBrandService
	{
		private readonly IBrandRepository _repository;
		private readonly IMapper _mapper;
		public BrandService(IBrandRepository brandRepository, IMapper mapper)
		{
			_repository = brandRepository;
			_mapper = mapper;
		}

		public async Task<List<BrandDto>> GetAllBrandsAsync()
		{
			var brand = await _repository.GetAllBrandsAsync();
			return _mapper.Map<List<BrandDto>>(brand);
		}
	}
}
