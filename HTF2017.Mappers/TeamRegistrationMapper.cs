using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTF2017.Mappers
{
    public class TeamRegistrationMapper
    {
        private readonly IMapper _mapper;

        public TeamRegistrationMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Team, TeamRegistrationDto>();
                cfg.CreateMap<TeamRegistrationDto, Team>();
            });
            _mapper = config.CreateMapper();
        }

        public TeamRegistrationDto Map(Team team)
        {
            return _mapper.Map<TeamRegistrationDto>(team);
        }

        public Team Map(TeamRegistrationDto team)
        {
            return _mapper.Map<Team>(team);
        }

        public List<TeamRegistrationDto> Map(List<Team> teams)
        {
            return teams.Select(Map).ToList();
        }

        public List<Team> Map(List<TeamRegistrationDto> teams)
        {
            return teams.Select(Map).ToList();
        }
    }
}