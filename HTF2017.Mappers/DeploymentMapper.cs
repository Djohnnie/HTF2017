using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;

namespace HTF2017.Mappers
{
    public class DeploymentMapper
    {
        private readonly IMapper _mapper;

        public DeploymentMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Android, DeployAndroidDto>();
                cfg.CreateMap<DeployAndroidDto, Android>();
                cfg.CreateMap<AutoPilot, AutoPilotDto>()
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => (Byte)src))
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToString()));
                cfg.CreateMap<AutoPilotDto, AutoPilot>()
                    .ConstructUsing(src => (AutoPilot)src.Code);
                cfg.CreateMap<SensorAccuracy, SensorAccuracyDto>()
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => (Byte)src))
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToString()));
                cfg.CreateMap<SensorAccuracyDto, SensorAccuracy>()
                    .ConstructUsing(src => (SensorAccuracy)src.Code);
            });
            _mapper = config.CreateMapper();
        }

        public DeployAndroidDto Map(Android team)
        {
            return _mapper.Map<DeployAndroidDto>(team);
        }

        public Android Map(DeployAndroidDto team)
        {
            return _mapper.Map<Android>(team);
        }

        public List<DeployAndroidDto> Map(List<Android> teams)
        {
            return teams.Select(Map).ToList();
        }

        public List<Android> Map(List<DeployAndroidDto> teams)
        {
            return teams.Select(Map).ToList();
        }
    }
}