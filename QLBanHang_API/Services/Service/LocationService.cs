using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;
using System.Runtime.CompilerServices;

namespace QLBanHang_API.Services.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;
        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            this.locationRepository = locationRepository;
            this.mapper = mapper;
        }

        //Get List
        public async Task<List<LocationDto>> GetListByName(string name)
        {
            var locations = await locationRepository.GetListByNameAsync(name);
            var locationsDto = mapper.Map<List<LocationDto>>(locations);
            return locationsDto;
        }

        //Get Location
        public async Task<LocationDto> GetLocationById(Guid id)
        {
            var location = await locationRepository.GetLocationByIdAsync(id);
            var locationDto = mapper.Map<LocationDto>(location);
            return locationDto;
        }

        //Add Location 
        public async Task<LocationDto> AddLocation(LocationRequest addLocation)
        {
            var location = mapper.Map<Location>(addLocation);
            var locationDomain = await locationRepository.AddLocationAsync(location);
            var locationDto = mapper.Map<LocationDto>(locationDomain);
            return locationDto;
        }

        //Update Location
        public async Task<LocationDto> UpdateLocation(Guid id,UpLocationDto upLocation)
        {
            var location = mapper.Map<Location>(upLocation);
            var locationDomain = await locationRepository.UpdateLocationAsync(id,location);
            var locationDto = mapper.Map<LocationDto>(locationDomain);
            return locationDto;
        }

        //Delete Location
        public async Task<LocationDto> DeleteLocation(Guid id)
        {
            var location = await locationRepository.DeleteLocationAsync(id);
            var locationDto = mapper.Map<LocationDto>(location);
            return locationDto;
        }
    }
}
