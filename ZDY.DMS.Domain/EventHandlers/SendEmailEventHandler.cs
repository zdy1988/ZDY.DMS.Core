using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Zdy.Events;
using ZDY.DMS;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Domain.Events;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Domain.EventHandlers
{
    [HandlesAsynchronously]
    public class SendEmailEventHandler : IEventHandler<SendEmailEvent>
    {
        private readonly IAppSettingService appSettingService;
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Log> logRepository;

        public SendEmailEventHandler(IRepositoryContext repositoryContext, IAppSettingService appSettingService)
        {
            this.repositoryContext = repositoryContext;
            this.logRepository = repositoryContext.GetRepository<Guid, Log>();
            this.appSettingService = appSettingService;
        }

        public void Handle(SendEmailEvent args)
        {
            try
            {
                string bodyContent = string.Format(@"
                    <div style='width: 700px;border: rgb(74, 143, 145) 2px;border-style: solid;padding: 10px;'>
                        <p style='text-align: center;'><img alt='LOGO' src='cid:aklogo'></p>
                        {0}
                    </div>", args.Content);
                MailMessage mail = new MailMessage(args.From, args.To, args.Title, bodyContent);
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true; //设置为HTML格式
                //设置LOGO
                string logo = "Application/Global/Img/logo-big.png";
                mail.Attachments.Add(new Attachment(logo, System.Net.Mime.MediaTypeNames.Application.Octet));
                mail.Attachments[0].ContentType.Name = "image/gif";
                mail.Attachments[0].ContentId = "aklogo";
                mail.Attachments[0].ContentDisposition.Inline = true;
                mail.Attachments[0].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                if (args.AttachmentPaths != null && args.AttachmentPaths.Count() > 0)
                {
                    foreach (string attachmentPath in args.AttachmentPaths)
                    {
                        mail.Attachments.Add(new Attachment(attachmentPath));
                    }
                }
                string host = this.appSettingService.GetAppSetting("EmailHost");
                string userName = this.appSettingService.GetAppSetting("EmailFrom");
                string pwd = this.appSettingService.GetAppSetting("EmailFromPswd");
                SmtpClient smtp = new SmtpClient(host);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(userName, pwd);
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                logRepository.AddAsync(new Log()
                {
                    TimeStamp = DateTime.Now,
                    Message = string.Format(exLogTemplate, this.appSettingService.GetAppSetting("EmailHost"), args.From, args.To, args.Title, args.Content, e.Message),
                    Type = (int)LogKinds.Email
                });
            }
        }

        private const string exLogTemplate = @"Host：{0}
                                           <br>来自：{1}
                                           <br>发送至：{2}
                                           <br>标题：{3}
                                           <br>内容：{4}
                                           <br>错误描述：{5}";
    }
}
