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
            _dbContext.Locations.Add(new Location { Name = "Central Europe", Lattitude = 49.9194008, Longitude = 6.0430397 });
            _dbContext.Locations.Add(new Location { Name = "Northern Europe", Lattitude = 61.9912145, Longitude = 12.6579873 });
            _dbContext.Locations.Add(new Location { Name = "Southern Europe", Lattitude = 41.3949627, Longitude = 2.0086849 });
            _dbContext.Locations.Add(new Location { Name = "Eastern Europe", Lattitude = 48.2444995, Longitude = 30.0446145 });
            _dbContext.Locations.Add(new Location { Name = "United States of America", Lattitude = 39.8646026, Longitude = -101.5089545 });
            _dbContext.Locations.Add(new Location { Name = "Canada", Lattitude = 59.5760083, Longitude = -112.109443 });
            _dbContext.Locations.Add(new Location { Name = "Central America", Lattitude = 14.7545596, Longitude = -87.475112 });
            _dbContext.Locations.Add(new Location { Name = "South America", Lattitude = -8.7831897, Longitude = -55.4936657 });
            _dbContext.Locations.Add(new Location { Name = "Russia", Lattitude = 61.2921444, Longitude = 89.5103138 });
            _dbContext.Locations.Add(new Location { Name = "New Zealand", Lattitude = -43.3392148, Longitude = 172.461096 });
            _dbContext.Locations.Add(new Location { Name = "Australia", Lattitude = -24.8535582, Longitude = 132.7571009 });
            _dbContext.Locations.Add(new Location { Name = "China", Lattitude = 34.7868527, Longitude = 102.0331196 });
            _dbContext.Locations.Add(new Location { Name = "Central Africa", Lattitude = 6.706757, Longitude = 20.0973021 });
            _dbContext.Locations.Add(new Location { Name = "South Africa", Lattitude = -33.9030916, Longitude = 18.4071074 });
            _dbContext.Locations.Add(new Location { Name = "United Arab Emirates", Lattitude = 23.9818299, Longitude = 53.9452593 });
            _dbContext.Locations.Add(new Location { Name = "Greenland", Lattitude = 66.7866238, Longitude = -46.9413866 });
            _dbContext.Locations.Add(new Location { Name = "India", Lattitude = 21.9761621, Longitude = 77.2527611 });
            _dbContext.Locations.Add(new Location { Name = "Japan", Lattitude = 35.7861952, Longitude = 138.8864704 });
            _dbContext.Locations.Add(new Location { Name = "Indonesia", Lattitude = -1.304056, Longitude = 113.9298059 });
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