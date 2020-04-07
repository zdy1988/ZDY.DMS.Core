using System;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.StringEncryption;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Events;
using ZDY.DMS.Services.Common.Events;

namespace ZDY.DMS.Services.UserService.Controllers
{
    public class UserController : ApiDataServiceController<Guid, User, UserServiceModule>
    {
        private readonly IStringEncryption stringEncryption;
        private readonly IEventPublisher eventPublisher;

        public UserController(Func<Type, IRepositoryContext> repositoryContextFactory,
            IStringEncryption stringEncryption, IEventPublisher eventPublisher)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {
            this.stringEncryption = stringEncryption;
            this.eventPublisher = eventPublisher;
        }

        protected override void BeforeAdd(User entity)
        {
            if (this.Repository.Exists(t => t.UserName == entity.UserName))
            {
                throw new InvalidOperationException("此账号已存在");
            }

            if (this.Repository.Exists(t => t.Mobile == entity.Mobile))
            {
                throw new InvalidOperationException("此手机号码已存在");
            }

            entity.Password = stringEncryption.Encrypt("123456");

            // 超级管理员提交的用户不设置公司ID
            if (!this.UserIdentity.IsAdministrator)
            {
                entity.CompanyId = this.UserIdentity.CompanyId;
            }
        }

        protected override void AfterAdd(User entity)
        {
            this.eventPublisher.Publish<UserCreatedEvent>(new UserCreatedEvent(entity.Id, entity.Name, entity.Password, entity.CompanyId));
        }

        protected override void BeforeUpdate(User original, User entity)
        {
            if (this.Repository.Exists(t => t.UserName == entity.UserName && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此账号已存在");
            }

            if (this.Repository.Exists(t => t.Mobile == entity.Mobile && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此手机号码已存在");
            }

            original.Avatar = entity.Avatar;
            original.DepartmentId = entity.DepartmentId;
            original.Email = entity.Email;
            original.Gender = entity.Gender;
            original.IsDisabled = entity.IsDisabled;
            original.Mobile = entity.Mobile;
            original.NickName = entity.NickName;
            original.Name = entity.Name;
            original.Phone = entity.Phone;
            original.Position = entity.Position;
            original.UserName = entity.UserName;
            original.City = entity.City;
            original.Country = entity.Country;
            original.Province = entity.Province;
        }
    }
}
