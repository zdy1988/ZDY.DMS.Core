﻿using System;
using Microsoft.AspNetCore.Authorization;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class PageController : ApiDataServiceController<Guid, Page, AdminServiceModule>
    {
        public PageController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {

        }
    }
}