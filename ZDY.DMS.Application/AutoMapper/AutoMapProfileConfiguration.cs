using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Models;

namespace ZDY.DMS.Application.AutoMapper
{
    public class AutoMapProfileConfiguration : Profile
    {
        public AutoMapProfileConfiguration()
        {
             CreateMap<BusinessOrderDataObject, BusinessOrder>();
             CreateMap<BusinessOrder, BusinessOrderDataObject>();
             CreateMap<BusinessOrderItemDataObject, BusinessOrderItem>();
             CreateMap<BusinessOrderItem, BusinessOrderItemDataObject>();
             CreateMap<BusinessOrderItemAttributeDataObject, BusinessOrderItemAttribute>();
             CreateMap<BusinessOrderItemAttribute, BusinessOrderItemAttributeDataObject>();
             CreateMap<CompanyDataObject, Company>();
             CreateMap<Company, CompanyDataObject>();
             CreateMap<CustomerDataObject, Customer>();
             CreateMap<Customer, CustomerDataObject>();
             CreateMap<DepartmentDataObject, Department>();
             CreateMap<Department, DepartmentDataObject>();
             CreateMap<DictionaryKeyDataObject, DictionaryKey>();
             CreateMap<DictionaryKey, DictionaryKeyDataObject>();
             CreateMap<DictionaryValueDataObject, DictionaryValue>();
             CreateMap<DictionaryValue, DictionaryValueDataObject>();
             CreateMap<FileDataObject, File>();
             CreateMap<File, FileDataObject>();
             CreateMap<GroupActionAuthorityDataObject, UserGroupActionPermission>();
             CreateMap<UserGroupActionPermission, GroupActionAuthorityDataObject>();
             CreateMap<GroupMemberDataObject, UserGroupMember>();
             CreateMap<UserGroupMember, GroupMemberDataObject>();
             CreateMap<GroupPageAuthorityDataObject, UserGroupPagePermission>();
             CreateMap<UserGroupPagePermission, GroupPageAuthorityDataObject>();
             CreateMap<GroupStructureDataObject, GroupStructure>();
             CreateMap<GroupStructure, GroupStructureDataObject>();
             CreateMap<LogDataObject, Log>();
             CreateMap<Log, LogDataObject>();
             CreateMap<MessageDataObject, Message>();
             CreateMap<Message, MessageDataObject>();
             CreateMap<MessagePullDataObject, MessageInbox>();
             CreateMap<MessageInbox, MessagePullDataObject>();
             CreateMap<MessageUserDataObject, MessageUser>();
             CreateMap<MessageUser, MessageUserDataObject>();
             CreateMap<PageDataObject, Page>();
             CreateMap<Page, PageDataObject>();
             CreateMap<PageActionDataObject, PageAction>();
             CreateMap<PageAction, PageActionDataObject>();
             CreateMap<ProductDataObject, Product>();
             CreateMap<Product, ProductDataObject>();
             CreateMap<SupplierDataObject, Supplier>();
             CreateMap<Supplier, SupplierDataObject>();
             CreateMap<UserDataObject, User>();
             CreateMap<User, UserDataObject>();
             CreateMap<UserGroupDataObject, UserGroup>();
             CreateMap<UserGroup, UserGroupDataObject>();
             CreateMap<WarehouseDataObject, Warehouse>();
             CreateMap<Warehouse, WarehouseDataObject>();
             CreateMap<WarehouseOrderDataObject, WarehouseOrder>();
             CreateMap<WarehouseOrder, WarehouseOrderDataObject>();
             CreateMap<WarehouseOrderItemDataObject, WarehouseOrderItem>();
             CreateMap<WarehouseOrderItem, WarehouseOrderItemDataObject>();
             CreateMap<WarehouseOrderItemAttributeDataObject, WarehouseOrderItemAttribute>();
             CreateMap<WarehouseOrderItemAttribute, WarehouseOrderItemAttributeDataObject>();
             CreateMap<WorkFlowDataObject, WorkFlow>();
             CreateMap<WorkFlow, WorkFlowDataObject>();
             CreateMap<WorkFlowCommentDataObject, WorkFlowComment>();
             CreateMap<WorkFlowComment, WorkFlowCommentDataObject>();
             CreateMap<WorkFlowDelegationDataObject, WorkFlowDelegation>();
             CreateMap<WorkFlowDelegation, WorkFlowDelegationDataObject>();
             CreateMap<WorkFlowFormDataObject, WorkFlowForm>();
             CreateMap<WorkFlowForm, WorkFlowFormDataObject>();
             CreateMap<WorkFlowInstanceDataObject, WorkFlowInstance>();
             CreateMap<WorkFlowInstance, WorkFlowInstanceDataObject>();
             CreateMap<WorkFlowTaskDataObject, WorkFlowTask>();
             CreateMap<WorkFlowTask, WorkFlowTaskDataObject>();
        }
    }
}
