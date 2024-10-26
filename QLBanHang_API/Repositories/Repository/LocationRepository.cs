using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Repositories.IRepository;
namespace QLBanHang_API.Repositories.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext dbContext;
        public LocationRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Tim kiem danh sach theo ten 
        public async Task<List<Location>> GetListByNameAsync(string? name)
        {
            var Locations = dbContext.Locations.AsQueryable();

            //Dieu kien neu name ko co 
            if (string.IsNullOrWhiteSpace(name) == false)
            {
                Locations = Locations.Where(x => x.Name.Contains(name));
            }
            return await Locations.ToListAsync();
        }

        // Lay thong tin  theo ID cua Location 
        public async Task<Location> GetLocationByIdAsync(Guid id)
        {
            var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
            if (location == null)
            {
                return null;
            }
            return location;
        }

        // THem Location 
        public async Task<Location> AddLocationAsync(Location location)
        {
            while (await dbContext.Locations.AnyAsync(x=> x.LocationId == location.LocationId))
            {
                location.LocationId = Guid.NewGuid();
            }
            await dbContext.Locations.AddAsync(location);
            await dbContext.SaveChangesAsync();
            return location;
        }

        // Update Location
        public async Task<Location> UpdateLocationAsync(Guid id, Location locationUpdate)
        {
            var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
            if (location == null)
            {
                return null;
            }
            location.Name = locationUpdate.Name;
            location.Description = locationUpdate.Description;
            location.YoutubeLink = locationUpdate.YoutubeLink;
            await dbContext.SaveChangesAsync();

            return location;
        }

        // Delete Location
        public async Task<Location> DeleteLocationAsync(Guid id)
        {
            var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
            if (location == null)
            {
                return null;
            }

            dbContext.Locations.Remove(location);
            await dbContext.SaveChangesAsync();
            return location;
        }
    }
}
