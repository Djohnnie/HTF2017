using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using HTF2017.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTF2017.Business
{
    public class LocationLogic
    {
        private readonly HtfDbContext _dbContext;
        private readonly LocationMapper _locationMapper;

        public LocationLogic(HtfDbContext dbContext, LocationMapper locationMapper)
        {
            _dbContext = dbContext;
            _locationMapper = locationMapper;
            _dbContext.Database.EnsureCreated();
        }

        public async Task AddDefaultLocations()
        {
            _dbContext.Locations.Add(new Location { Name = "Central Europe" });
            _dbContext.Locations.Add(new Location { Name = "Northern Europe" });
            _dbContext.Locations.Add(new Location { Name = "Southern Europe" });
            _dbContext.Locations.Add(new Location { Name = "Eastern Europe" });
            _dbContext.Locations.Add(new Location { Name = "United States of America" });
            _dbContext.Locations.Add(new Location { Name = "Canada" });
            _dbContext.Locations.Add(new Location { Name = "Central America" });
            _dbContext.Locations.Add(new Location { Name = "South America" });
            _dbContext.Locations.Add(new Location { Name = "Russia" });
            _dbContext.Locations.Add(new Location { Name = "New Zealand" });
            _dbContext.Locations.Add(new Location { Name = "Australia" });
            _dbContext.Locations.Add(new Location { Name = "China" });
            _dbContext.Locations.Add(new Location { Name = "Central Africa" });
            _dbContext.Locations.Add(new Location { Name = "South Africa" });
            _dbContext.Locations.Add(new Location { Name = "United Arab Emirates" });
            _dbContext.Locations.Add(new Location { Name = "Greenland" });
            _dbContext.Locations.Add(new Location { Name = "India" });
            _dbContext.Locations.Add(new Location { Name = "Japan" });
            _dbContext.Locations.Add(new Location { Name = "Indonesia" });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<LocationDto>> GetAllLocations()
        {
            if (await _dbContext.Locations.CountAsync() == 0)
            {
                await AddDefaultLocations();
            }
            List<Location> locations = await _dbContext.Locations.ToListAsync();
            return _locationMapper.Map(locations);
        }
    }
}