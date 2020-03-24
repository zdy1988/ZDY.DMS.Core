using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.DataTransferObjects;
using ZDY.DMS.Models;

namespace ZDY.DMS.Application.AutoMapper
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
