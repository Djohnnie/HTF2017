using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace HTF2017.Mappers
{
    public class LocationMapper
    {
        private readonly IMapper _mapper;

        public LocationMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Location, LocationDto>();
                cfg.CreateMap<LocationDto, Location>();
            });
            _mapper = config.CreateMapper();
        }

        public LocationDto Map(Location location)
        {
            return _mapper.Map<LocationDto>(location);
        }

        public Location Map(LocationDto location)
        {
            return _mapper.Map<Location>(location);
        }

        public List<LocationDto> Map(List<Location> locations)
        {
            return locations.Select(Map).ToList();
        }

        public List<Location> Map(List<LocationDto> locations)
        {
            return locations.Select(Map).ToList();
        }
    }
}