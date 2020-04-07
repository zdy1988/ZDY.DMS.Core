using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.StringEncryption;

namespace ZDY.DMS.Services.WorkFlowService.Controllers
{
    public class WorkFlowSignatureController : ApiDataServiceController<Guid, WorkFlowSignature, WorkFlowServiceModule>
    {
        private readonly IStringEncryption stringEncryption;

        public WorkFlowSignatureController(Func<Type, IRepositoryContext> repositoryContextFactory,
            IStringEncryption stringEncryption)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {
            this.stringEncryption = stringEncryption;
        }

        protected override void BeforeAdd(WorkFlowSignature entity)
        {
            entity.Password = stringEncryption.Encrypt(entity.Password);

            if (Repository.Exists(t => t.UserId == this.UserIdentity.Id && t.Password == entity.Password))
            {
                throw new InvalidOperationException("此签名密钥已存在，请另考虑其他密钥");
            }
        }

        protected override void BeforeUpdate(WorkFlowSignature original, WorkFlowSignature entity)
        {
            if (original.Password != entity.Password)
            {
                entity.Password = stringEncryption.Encrypt(entity.Password);

                if (Repository.Exists(t => t.UserId == this.UserIdentity.Id && t.Password == entity.Password))
                {
                    throw new InvalidOperationException("此签名密钥已存在，请另考虑其他密钥");
                }

                original.Password = entity.Password;
            }

            original.Name = entity.Name;
            original.Note = entity.Note;
        }
    }
}
