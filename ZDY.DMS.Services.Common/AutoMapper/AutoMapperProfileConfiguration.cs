using AutoMapper;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.Common.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<MultiLevelPageDTO, Page>();
            CreateMap<Page, MultiLevelPageDTO>();
        }
    }
}
