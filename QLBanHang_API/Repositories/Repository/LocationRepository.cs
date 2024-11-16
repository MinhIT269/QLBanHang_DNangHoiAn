using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using Microsoft.EntityFrameworkCore;
using QLBanHang_API.Repositories.IRepository;
using System.Net;
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
                Locations = Locations.Where(p => EF.Functions.Collate(p.Name!, "SQL_Latin1_General_CP1_CI_AI").Contains(name.Trim()));
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

        // Update List Location
        public async Task<List<Location>> UpdateListLocationAsync(List<Location> locationUpdate)
        {
            var locations = new List<Location>();
            foreach (Location item in locationUpdate)
            {
                var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.LocationId == item.LocationId);
                if (location == null)
                {
                    continue;
                }
                location.Name = item.Name;
                location.Description = item.Description;
                location.YoutubeLink = item.YoutubeLink;
                
                locations.Add(location);
            }
            await dbContext.SaveChangesAsync();
            return locations;
        }

        //Update Location
        public async Task<Location> UpdateLocationAsync(Location locationUpdate)
        {
            var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.LocationId == locationUpdate.LocationId);
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
