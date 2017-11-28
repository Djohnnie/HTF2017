using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;

namespace HTF2017.Mappers
{
    public class AndroidMapper
    {
        private readonly IMapper _mapper;

        public AndroidMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Android, AndroidDto>();
                cfg.CreateMap<AndroidDto, Android>();
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

        public AndroidDto Map(Android team)
        {
            return _mapper.Map<AndroidDto>(team);
        }

        public Android Map(AndroidDto team)
        {
            return _mapper.Map<Android>(team);
        }

        public List<AndroidDto> Map(List<Android> teams)
        {
            return teams.Select(Map).ToList();
        }

        public List<Android> Map(List<AndroidDto> teams)
        {
            return teams.Select(Map).ToList();
        }
    }
}