using AutoMapper;
using ZDY.DMS.Services.AdminService.DataTransferObjects;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService
{
    public class AdminServiceProfile: Profile
    {
        public AdminServiceProfile()
        {
            CreateMap<MultiLevelPageDTO, Page>()
                .ReverseMap();
        }
    }
}
