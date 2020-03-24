using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Domain.Models;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.StringEncryption;

namespace ZDY.DMS.Services.AuthService
{

    public class AuthController : ApiController
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringEncryption stringEncryption;
        private readonly IRepository<Guid, User> userRepository;
        private readonly IRepository<Guid, Company> companyRepository;

        public AuthController(IRepositoryContext repositoryContext,
            IHttpContextAccessor httpContextAccessor,
            IStringEncryption stringEncryption)
        {
            this.repositoryContext = repositoryContext;
            this.httpContextAccessor = httpContextAccessor;
            this.stringEncryption = stringEncryption;
            this.userRepository = repositoryContext.GetRepository<Guid, User>();
            this.companyRepository = repositoryContext.GetRepository<Guid, Company>();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetSecurityToken(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("账号或密码有误");
            }

            password = stringEncryption.Encrypt(password);

            var existUser = await userRepository.FindAsync(t => (t.Mobile == account || t.UserName == account) && t.Password == password);

            if (existUser == null)
            {
                throw new InvalidOperationException("账号或密码有误");
            }

            if (existUser.IsDisabled)
            {
                throw new InvalidOperationException("账号已被禁用");
            }

            if (existUser.CompanyId != this.HttpContext.GetMysteriousCode())
            {
                var existCompany = await companyRepository.FindAsync(t => t.Id == existUser.CompanyId);

                if (existCompany == null)
                {
                    throw new InvalidOperationException("无效公司或者公司已注销");
                }
            }

            var requestAt = DateTime.Now;
            var expiresIn = requestAt + SecurityTokenOptions.ExpiresSpan;
            var token = UserAuthorization.GenerateToken(existUser.Id, existUser.NickName, existUser.CompanyId, expiresIn);

            //记录登陆信息
            existUser.LastLoginTime = DateTime.Now;
            existUser.LastLoginIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            existUser.Session = token;
            await userRepository.UpdateAsync(existUser);

            return Ok(new
            {
                requestAt = requestAt,
                expiresIn = SecurityTokenOptions.ExpiresSpan.TotalSeconds,
                accessToken = token,
                userInfo = AssignUserInfo(existUser)
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetWeChatAuthToken(WechatLoginInfo wechatLoginInfo)
        {
            const string appId = "wxe7e98caf73da8a57";
            const string appSecret = "87f3f99d3cd3992058cdc4a254a43109";
            WeChatAppDecrypt weChatAppDecrypt = new WeChatAppDecrypt(appId, appSecret);
            var wechatUserInfo = weChatAppDecrypt.Decrypt(wechatLoginInfo);

            if (string.IsNullOrEmpty(wechatUserInfo.openId))
            {
                throw new InvalidOperationException("微信授权信息有误");
            }

            User existUser = await userRepository.FindAsync(t => t.WeChatOpenId == wechatUserInfo.openId);

            if (existUser == null)
            {
                existUser = new User
                {
                    Mobile = "未设置",
                    AvatarUrl = wechatUserInfo.avatarUrl,
                    NickName = wechatUserInfo.nickName,
                    Country = wechatUserInfo.country,
                    Province = wechatUserInfo.province,
                    City = wechatUserInfo.city,
                    WeChatOpenId = wechatUserInfo.openId,
                    Gender = wechatUserInfo.gender == "1" ? "男" : "女"
                };
                await userRepository.AddAsync(existUser);
            }

            if (existUser.IsDisabled)
            {
                throw new InvalidOperationException("账号已被禁用");
            }

            if (existUser.CompanyId != default(Guid))
            {
                var existCompany = await companyRepository.FindByKeyAsync(existUser.CompanyId);

                if (existCompany == null)
                {
                    throw new InvalidOperationException("无效公司或者公司已注销");
                }
            }

            var requestAt = DateTime.Now;
            var expiresIn = requestAt + SecurityTokenOptions.ExpiresSpan;
            var token = UserAuthorization.GenerateToken(existUser.Id, existUser.NickName, existUser.CompanyId, expiresIn);

            existUser.AvatarUrl = wechatUserInfo.avatarUrl;
            existUser.NickName = wechatUserInfo.nickName;
            existUser.Country = wechatUserInfo.country;
            existUser.Province = wechatUserInfo.province;
            existUser.City = wechatUserInfo.city;
            existUser.WeChatOpenId = wechatUserInfo.openId;
            existUser.Gender = wechatUserInfo.gender == "1" ? "男" : "女";
            existUser.LastLoginTime = DateTime.Now;
            existUser.LastLoginIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            existUser.Session = token;

            await userRepository.UpdateAsync(existUser);

            return Ok(new
            {
                requestAt = requestAt,
                expiresIn = SecurityTokenOptions.ExpiresSpan.TotalSeconds,
                accessToken = token,
                userInfo = AssignUserInfo(existUser)
            });
        }

        [HttpPost]
        public async Task ChangePassword(string orgPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(orgPassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new InvalidOperationException("密码不正确");
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;

            var id = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "ID").Value;

            var user = await userRepository.FindAsync(t => t.Password == stringEncryption.Encrypt(orgPassword) && t.Id == Guid.Parse(id));

            if (user == null)
            {
                throw new InvalidOperationException("密码不正确");
            }

            user.Password = stringEncryption.Encrypt(newPassword);

            userRepository.Update(user);

            await userRepository.Context.CommitAsync();
        }

        [HttpPost]
        public async Task<User> GetCurrentUser()
        {
            var id = this.UserIdentity.Id;

            var existUser = await userRepository.FindByKeyAsync(id);

            if (existUser == null)
            {
                throw new InvalidOperationException("无此用户");
            }

            return AssignUserInfo(existUser);
        }

        private User AssignUserInfo(User existUser)
        {
            return new User
            {
                Id = existUser.Id,
                Avatar = existUser.Avatar,
                AvatarUrl = existUser.GetUserAvatarUrl(),
                UserName = existUser.UserName,
                NickName = existUser.NickName,
                Gender = existUser.Gender,
                Country = existUser.Country,
                City = existUser.City,
                Province = existUser.Province,
                Email = existUser.Email,
                Phone = existUser.Phone,
                Mobile = existUser.Mobile
            };
        }
    }
}
