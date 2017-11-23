using System.Collections.Generic;
using System.Threading.Tasks;
using HTF2017.DataAccess;
using Microsoft.EntityFrameworkCore;
using HTF2017.Mappers;
using HTF2017.DataTransferObjects;
using System.Linq;
using System;

namespace HTF2017.Business
{
    public class TeamLogic
    {
        private readonly HtfDbContext _dbContext;
        private readonly TeamMapper _teamMapper;
        private readonly TeamRegistrationMapper _teamRegistrationMapper;

        public TeamLogic(HtfDbContext dbContext, TeamMapper teamMapper, TeamRegistrationMapper teamRegistrationMapper)
        {
            _dbContext = dbContext;
            _teamMapper = teamMapper;
            _teamRegistrationMapper = teamRegistrationMapper;
            _dbContext.Database.EnsureCreated();
        }

        public async Task<List<TeamDto>> GetAllTeams()
        {
            return await _dbContext.Teams.Include(x => x.Location).Select(x => new TeamDto
            {
                Id = x.Id,
                Name = x.Name,
                FeedbackEndpoint = x.FeedbackEndpoint,
                Score = x.Score,
                LocationId = x.Location.Id,
                LocationName = x.Location.Name,
                TotalNumberOfAndroids = x.TotalNumberOfAndroids,
                NumberOfAndroidsAvailable = x.TotalNumberOfAndroids - x.Androids.Count,
                NumberOfAndroidsActive = x.Androids.Count(a => !a.Compomised),
                NumberOfAndroidsCompromised = x.Androids.Count(a => a.Compomised),
            }).ToListAsync();
        }

        public async Task<TeamDto> RegisterTeam(TeamRegistrationDto team)
        {
            Team teamToRegister = _teamRegistrationMapper.Map(team);
            List<Location> availableLocations = await _dbContext.Locations.Where(x => x.Teams.Count == 0).ToListAsync();
            teamToRegister.Location = availableLocations.RandomSingle();
            if (teamToRegister.Location == null)
            {
                throw new Exception("No locations available");
            }
            teamToRegister.TotalNumberOfAndroids = 1000;
            _dbContext.Teams.Add(teamToRegister);
            await _dbContext.SaveChangesAsync();
            TeamDto registeredTeam = _teamMapper.Map(teamToRegister);
            registeredTeam.NumberOfAndroidsAvailable = teamToRegister.TotalNumberOfAndroids;
            return registeredTeam;
        }

        public async Task<TeamDto> UpdateTeam(Guid teamId, TeamRegistrationDto team)
        {
            Team teamToUpdate = await _dbContext.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
            teamToUpdate.Name = team.Name;
            teamToUpdate.FeedbackEndpoint = team.FeedbackEndpoint;
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Teams.Include(x => x.Location).Select(x => new TeamDto
            {
                Id = x.Id,
                Name = x.Name,
                FeedbackEndpoint = x.FeedbackEndpoint,
                Score = x.Score,
                LocationId = x.Location.Id,
                LocationName = x.Location.Name,
                TotalNumberOfAndroids = x.TotalNumberOfAndroids,
                NumberOfAndroidsAvailable = x.TotalNumberOfAndroids - x.Androids.Count,
                NumberOfAndroidsActive = x.Androids.Count(a => !a.Compomised),
                NumberOfAndroidsCompromised = x.Androids.Count(a => a.Compomised),
            }).SingleOrDefaultAsync();
        }
    }
}