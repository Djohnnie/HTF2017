using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2017.Business.Exceptions;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;
using HTF2017.Mappers;
using Microsoft.EntityFrameworkCore;
using Crypt = BCrypt.Net.BCrypt;

namespace HTF2017.Business
{
    public class AndroidLogic
    {
        private readonly HtfDbContext _dbContext;
        private readonly AndroidMapper _androidMapper;
        private readonly DeploymentMapper _deploymentMapper;
        private readonly RequestMapper _requestMapper;

        public AndroidLogic(
            HtfDbContext dbContext,
            AndroidMapper androidMapper,
            DeploymentMapper deploymentMapper,
            RequestMapper requestMapper)
        {
            _dbContext = dbContext;
            _androidMapper = androidMapper;
            _deploymentMapper = deploymentMapper;
            _requestMapper = requestMapper;
            _dbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets all deployed androids in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to get the androids for.</param>
        /// <returns>A list of requested androids.</returns>
        public async Task<List<AndroidDto>> GetAndroids(Guid teamId)
        {
            List<Android> androids = await _dbContext.Androids.Where(x => x.TeamId == teamId).ToListAsync();
            return _androidMapper.Map(androids);
        }

        /// <summary>
        /// Deploys a new android in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team to deploy an android for.</param>
        /// <param name="android">The android settings to use for deployment.</param>
        /// <returns>The deployed android.</returns>
        public async Task<AndroidDto> DeployAndroid(Guid teamId, DeployAndroidDto android)
        {
            Team teamToCheck = await _dbContext.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
            if (teamToCheck == null) { throw new HtfValidationException("The specified team is unknown!"); }
            if (!Crypt.EnhancedVerify(android.Password, teamToCheck.Password)) { throw new HtfValidationException("The specified password is not correct!"); }

            Android androidToDeploy = _deploymentMapper.Map(android);
            androidToDeploy.TeamId = teamId;
            if (androidToDeploy.AutoPilot == AutoPilot.Level1)
            {
                androidToDeploy.LocationSensorAccuracy = SensorAccuracy.LowAccuracySensor;
                androidToDeploy.CrowdSensorAccuracy = SensorAccuracy.LowAccuracySensor;
                androidToDeploy.MoodSensorAccuracy = SensorAccuracy.LowAccuracySensor;
                androidToDeploy.RelationshipSensorAccuracy = SensorAccuracy.LowAccuracySensor;
            }
            switch (androidToDeploy.AutoPilot)
            {
                case AutoPilot.Level1:
                    teamToCheck.Score += 10;
                    break;
                case AutoPilot.Level2:
                    teamToCheck.Score += 100;
                    break;
                case AutoPilot.Level3:
                    teamToCheck.Score += 1000;
                    break;
            }
            await _dbContext.Androids.AddAsync(androidToDeploy);
            await _dbContext.SaveChangesAsync();

            return _androidMapper.Map(androidToDeploy);
        }

        /// <summary>
        /// Deploys a new android in the field.
        /// </summary>
        /// <param name="teamId">The unique identifier of the registered team that is sending this request.</param>
        /// <param name="androidId">The unique identifier of the deployed android this reuest is meant for.</param>
        /// <param name="request">The actual request.</param>
        /// <returns>The android the request is for.</returns>
        public async Task<AndroidDto> SendRequest(Guid teamId, Guid androidId, AndroidRequestDto request)
        {
            Team teamToCheck = await _dbContext.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
            if (teamToCheck == null) { throw new HtfValidationException("The specified team is unknown!"); }
            if (!Crypt.EnhancedVerify(request.Password, teamToCheck.Password)) { throw new HtfValidationException("The specified password is not correct!"); }
            Android androidToRequest = await _dbContext.Androids.SingleOrDefaultAsync(x => x.Id == androidId);
            if (androidToRequest == null) { throw new HtfValidationException("The specified android is unknown!"); }
            if(androidToRequest.AutoPilot == AutoPilot.Level1) { throw new HtfValidationException("The specified level-1 android does not support manual requests!"); }

            SensoryDataRequest requestToCreate = _requestMapper.Map(request);
            requestToCreate.AndroidId = androidId;
            requestToCreate.TimeStamp = DateTime.UtcNow;
            await _dbContext.SensoryDataRequests.AddAsync(requestToCreate);
            await _dbContext.SaveChangesAsync();

            return _androidMapper.Map(androidToRequest);
        }
    }
}