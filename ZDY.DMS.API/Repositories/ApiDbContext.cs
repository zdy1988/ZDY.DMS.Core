using System;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.Services.AdminService.Models;
using ZDY.DMS.Services.MessageService.Models;
using ZDY.DMS.Services.OrganizationService.Models;
using ZDY.DMS.Services.PermissionService.Models;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.UserService.Models;

namespace ZDY.DMS.API.Repositories.EntityFramework
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
                
        }

        public DbSet<UserGroup> UserGroup { set; get; }
        public DbSet<User> User { set; get; }
        public DbSet<Page> Page { set; get; }
        public DbSet<PageAction> PageAction { set; get; }
        public DbSet<Log> Log { set; get; }
        public DbSet<UserGroupPagePermission> UserGroupPageAuthority { set; get; }
        public DbSet<UserGroupMember> UserGroupMember { set; get; }
        public DbSet<UserGroupActionPermission> UserGroupActionPermission { set; get; }
        public DbSet<DictionaryValue> DictionaryValue { set; get; }
        public DbSet<DictionaryKey> DictionaryKey { set; get; }
        public DbSet<Department> Department { set; get; }
        public DbSet<Company> Company { set; get; }
        public DbSet<File> File { set; get; }

        public DbSet<WorkFlow> WorkFlow { set; get; }
        public DbSet<WorkFlowComment> WorkFlowComment { set; get; }
        public DbSet<WorkFlowDelegation> WorkFlowDelegation { set; get; }
        public DbSet<WorkFlowForm> WorkFlowForm { set; get; }
        public DbSet<WorkFlowInstance> WorkFlowInstance { set; get; }
        public DbSet<WorkFlowTask> WorkFlowTask { set; get; }

        public DbSet<Message> Message { set; get; }
        public DbSet<MessageInbox> MessageInbox { set; get; }


        //public DbSet<BusinessOrder> BusinessOrder { set; get; }
        //public DbSet<BusinessOrderItem> BusinessOrderItem { set; get; }
        //public DbSet<BusinessOrderItemAttribute> BusinessOrderItemAttribute { set; get; }
        //public DbSet<Customer> Customer { set; get; }
        //public DbSet<Product> Product { set; get; }
        //public DbSet<Supplier> Supplier { set; get; }
        //public DbSet<Warehouse> Warehouse { set; get; }
        //public DbSet<WarehouseOrder> WarehouseOrder { set; get; }
        //public DbSet<WarehouseOrderItem> WarehouseOrderItem { set; get; }
        //public DbSet<WarehouseOrderItemAttribute> WarehouseOrderItemAttribute { set; get; }
    }
}
