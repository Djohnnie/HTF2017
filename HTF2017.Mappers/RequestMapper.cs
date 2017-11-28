using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HTF2017.DataAccess;
using HTF2017.DataTransferObjects;

namespace HTF2017.Mappers
{
    public class RequestMapper
    {
        private readonly IMapper _mapper;

        public RequestMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SensoryDataRequest, AndroidRequestDto>();
                cfg.CreateMap<DeployAndroidDto, SensoryDataRequest>();
            });
            _mapper = config.CreateMapper();
        }

        public AndroidRequestDto Map(SensoryDataRequest request)
        {
            return _mapper.Map<AndroidRequestDto>(request);
        }

        public SensoryDataRequest Map(AndroidRequestDto team)
        {
            return _mapper.Map<SensoryDataRequest>(team);
        }

        public List<AndroidRequestDto> Map(List<SensoryDataRequest> requests)
        {
            return requests.Select(Map).ToList();
        }

        public List<SensoryDataRequest> Map(List<AndroidRequestDto> requests)
        {
            return requests.Select(Map).ToList();
        }
    }
}