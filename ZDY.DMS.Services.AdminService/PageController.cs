using Microsoft.AspNetCore.Authorization;
using System;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.AdminService
{
    //[Authorize(Roles = "Administrator")]
    public class PageController : ApiDataServiceController<Guid, Page>
    {
        public PageController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {

        }
    }
}
