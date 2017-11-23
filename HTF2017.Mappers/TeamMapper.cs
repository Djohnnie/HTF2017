using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace HTF2017.Mappers
{
    public class TeamMapper
    {
        private readonly IMapper _mapper;

        public TeamMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Team, TeamDto>();
                cfg.CreateMap<TeamDto, Team>();
            });
            _mapper = config.CreateMapper();
        }

        public TeamDto Map(Team location)
        {
            return _mapper.Map<TeamDto>(location);
        }

        public Team Map(TeamDto location)
        {
            return _mapper.Map<Team>(location);
        }

        public List<TeamDto> Map(List<Team> locations)
        {
            return locations.Select(Map).ToList();
        }

        public List<Team> Map(List<TeamDto> locations)
        {
            return locations.Select(Map).ToList();
        }
    }
}