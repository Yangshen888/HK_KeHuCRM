using System;
using HaikanCRM.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HaikanCRM.Api.Entities
{
    public partial class HaikanCRMContext : DbContext
    {
        public HaikanCRMContext()
        {
        }

        public HaikanCRMContext(DbContextOptions<HaikanCRMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressBook> AddressBook { get; set; }
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<BusinessDocuments> BusinessDocuments { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<ContactCallLog> ContactCallLog { get; set; }
        public virtual DbSet<ContactCallLogView> ContactCallLogView { get; set; }
        public virtual DbSet<ContactDetails> ContactDetails { get; set; }
        public virtual DbSet<ContactPerson> ContactPerson { get; set; }
        public virtual DbSet<ContactPersonView> ContactPersonView { get; set; }
        public virtual DbSet<ContactPlist> ContactPlist { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerIndustry> CustomerIndustry { get; set; }
        public virtual DbSet<CustomerStatus> CustomerStatus { get; set; }
        public virtual DbSet<CustomerTree> CustomerTree { get; set; }
        public virtual DbSet<CustomerTypeList> CustomerTypeList { get; set; }
        public virtual DbSet<SystemDepartment> SystemDepartment { get; set; }
        public virtual DbSet<SystemIcon> SystemIcon { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<SystemMenu> SystemMenu { get; set; }
        public virtual DbSet<SystemMessage> SystemMessage { get; set; }
        public virtual DbSet<SystemPermission> SystemPermission { get; set; }
        public virtual DbSet<SystemRole> SystemRole { get; set; }
        public virtual DbSet<SystemRolePermissionMapping> SystemRolePermissionMapping { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<SystemUserRoleMapping> SystemUserRoleMapping { get; set; }
        public virtual DbSet<ViewMenu> ViewMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu> ViewSystemPermissionWithMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu2> ViewSystemPermissionWithMenu2 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.214.;Initial Catalog=haikanCRM;Persist Security Info=True;User ID=haikanCRM;Password=haikan051030");
                var conncectstr = ConfigurationManager.ConnectionStrings.DefaultConnection;
                optionsBuilder.UseSqlServer(conncectstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBook>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("AddressBook");

                entity.Property(e => e.RealName).HasMaxLength(255);

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.Txlu).HasColumnName("txlu");
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.BusinessUuid);

                entity.HasComment("商机表");

                entity.Property(e => e.BusinessUuid)
                    .HasColumnName("BusinessUUID")
                    .HasComment("商机UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(255)
                    .HasComment("商机名称");

                entity.Property(e => e.BusinessSource)
                    .HasMaxLength(255)
                    .HasComment("商机来源");

                entity.Property(e => e.BusinessStage)
                    .HasMaxLength(100)
                    .HasComment("商机阶段");

                entity.Property(e => e.BusinessType)
                    .HasMaxLength(100)
                    .HasComment("商机类型");

                entity.Property(e => e.CheckTime)
                    .HasMaxLength(255)
                    .HasComment("预计成交时间");

                entity.Property(e => e.ClientUuid)
                    .HasColumnName("ClientUUID")
                    .HasComment("客户UUID");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID");

                entity.Property(e => e.ContactBusName).HasComment("相关联系人");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Remarks).HasComment("备注");

                entity.Property(e => e.SalesAmount)
                    .HasMaxLength(255)
                    .HasComment("项目预算");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasComment("状态");

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.Winrate)
                    .HasMaxLength(100)
                    .HasComment("商机赢率");

                entity.HasOne(d => d.ClientUu)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.ClientUuid)
                    .HasConstraintName("FK_Business_Client");
            });

            modelBuilder.Entity<BusinessDocuments>(entity =>
            {
                entity.HasKey(e => e.BusDocumentsUuid);

                entity.HasComment("商机文件");

                entity.Property(e => e.BusDocumentsUuid)
                    .HasColumnName("BusDocumentsUUID")
                    .HasComment("文件UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime).HasComment("上传时间");

                entity.Property(e => e.BusinessUuid)
                    .HasColumnName("BusinessUUID")
                    .HasComment("所属商机的UUID");

                entity.Property(e => e.ClientUuid)
                    .HasColumnName("ClientUUID")
                    .HasComment("所属客户的UUID");

                entity.Property(e => e.ContactPersonUuid).HasColumnName("ContactPersonUUID");

                entity.Property(e => e.FileInfo).HasComment("文件简介");

                entity.Property(e => e.FileName).HasComment("文件名称");

                entity.Property(e => e.FileUrl).HasComment("保存路径");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("状态");

                entity.HasOne(d => d.BusinessUu)
                    .WithMany(p => p.BusinessDocuments)
                    .HasForeignKey(d => d.BusinessUuid)
                    .HasConstraintName("FK_BusinessDocuments_Business");

                entity.HasOne(d => d.ClientUu)
                    .WithMany(p => p.BusinessDocuments)
                    .HasForeignKey(d => d.ClientUuid)
                    .HasConstraintName("FK_BusinessDocuments_Client");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyUuid);

                entity.HasComment("公司表");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessLogin)
                    .HasMaxLength(255)
                    .HasComment("工商注册号");

                entity.Property(e => e.CompanyAddress)
                    .HasMaxLength(255)
                    .HasComment("企业地址");

                entity.Property(e => e.CompanyEmail)
                    .HasMaxLength(255)
                    .HasComment("企业邮箱");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .HasComment("公司名称");

                entity.Property(e => e.CompanyPhone)
                    .HasMaxLength(100)
                    .HasComment("企业联系电话 ");

                entity.Property(e => e.CompanyType)
                    .HasMaxLength(100)
                    .HasComment("企业类型");

                entity.Property(e => e.District)
                    .HasMaxLength(100)
                    .HasComment("所属地区");

                entity.Property(e => e.EngName)
                    .HasMaxLength(255)
                    .HasComment("英文名 ");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Industry)
                    .HasColumnName("industry")
                    .HasMaxLength(100)
                    .HasComment("所属行业");

                entity.Property(e => e.LegalPeople)
                    .HasMaxLength(100)
                    .HasComment("法定代表人");

                entity.Property(e => e.Officialwebsite)
                    .HasMaxLength(255)
                    .HasComment("企业官网");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasComment("状态");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户UUID");

                entity.Property(e => e.UnifiedSocialCode)
                    .HasMaxLength(255)
                    .HasComment("统一社会信用代码");
            });

            modelBuilder.Entity<ContactCallLog>(entity =>
            {
                entity.HasKey(e => e.CallLogUuid)
                    .HasName("PK_CallLog");

                entity.HasComment("联系记录表");

                entity.Property(e => e.CallLogUuid)
                    .HasColumnName("CallLogUUID")
                    .HasComment("联系记录UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessUuid)
                    .HasColumnName("BusinessUUID")
                    .HasComment("商机UUID");

                entity.Property(e => e.CallContent).HasComment("联系目的,联系内容情况及结果");

                entity.Property(e => e.CallTime)
                    .HasMaxLength(100)
                    .HasComment("联系时间");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(255)
                    .HasComment("客户名称");

                entity.Property(e => e.ClientUuid)
                    .HasColumnName("ClientUUID")
                    .HasComment("客户UUID");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID");

                entity.Property(e => e.ContactDetailsName).HasMaxLength(100);

                entity.Property(e => e.ContactDetailsUuid).HasColumnName("ContactDetailsUUID");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasComment("联系人名称");

                entity.Property(e => e.ContactPersonUuid)
                    .HasColumnName("ContactPersonUUID")
                    .HasComment("联系人UUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Remarks).HasComment("备注");

                entity.Property(e => e.SheBeiId)
                    .HasColumnName("SheBeiID")
                    .HasComment("设备Id");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasComment("状态");

                entity.HasOne(d => d.BusinessUu)
                    .WithMany(p => p.ContactCallLog)
                    .HasForeignKey(d => d.BusinessUuid)
                    .HasConstraintName("FK_CallLog_Business");

                entity.HasOne(d => d.ClientUu)
                    .WithMany(p => p.ContactCallLog)
                    .HasForeignKey(d => d.ClientUuid)
                    .HasConstraintName("FK_CallLog_Client");

                entity.HasOne(d => d.ContactPersonUu)
                    .WithMany(p => p.ContactCallLog)
                    .HasForeignKey(d => d.ContactPersonUuid)
                    .HasConstraintName("FK_CallLog_ContactPerson");
            });

            modelBuilder.Entity<ContactCallLogView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContactCallLogView");

                entity.Property(e => e.CallLogUuid).HasColumnName("CallLogUUID");

                entity.Property(e => e.CallTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContactDetails>(entity =>
            {
                entity.HasKey(e => e.ContactDetailsUuid);

                entity.HasComment("联系方式表");

                entity.Property(e => e.ContactDetailsUuid)
                    .HasColumnName("ContactDetailsUUID")
                    .HasComment("联系方式UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cellphone)
                    .HasMaxLength(100)
                    .HasComment("座机号码");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .HasComment("手机号码");

                entity.Property(e => e.TencentQq)
                    .HasColumnName("TencentQQ")
                    .HasMaxLength(100)
                    .HasComment("QQ");

                entity.Property(e => e.WeChat)
                    .HasMaxLength(100)
                    .HasComment("微信 ");
            });

            modelBuilder.Entity<ContactPerson>(entity =>
            {
                entity.HasKey(e => e.ContactPersonUuid)
                    .IsClustered(false);

                entity.HasComment("联系人表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_ContactPerson")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ContactPersonUuid)
                    .HasColumnName("ContactPersonUUID")
                    .HasComment("联系人UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.BgPhone)
                    .HasColumnName("bgPhone")
                    .HasMaxLength(100)
                    .HasComment("手机号码");

                entity.Property(e => e.Call)
                    .HasMaxLength(100)
                    .HasComment("称呼");

                entity.Property(e => e.CallLogUuid)
                    .HasColumnName("CallLogUUID")
                    .HasComment("联系记录UUID");

                entity.Property(e => e.Cellphone)
                    .HasMaxLength(100)
                    .HasComment("手机");

                entity.Property(e => e.ClientUuid)
                    .HasColumnName("ClientUUID")
                    .HasComment("客户UUID");

                entity.Property(e => e.CompanySize).HasComment("公司规模");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID");

                entity.Property(e => e.ContactDetailsUuid)
                    .HasColumnName("ContactDetailsUUID")
                    .HasComment("联系方式UUID");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasComment("联系人名称");

                entity.Property(e => e.DateBirth).HasComment("出生日期");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasComment("邮件");

                entity.Property(e => e.FamilyAddress).HasComment("家庭地址");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OfficeAddress).HasComment("办公地址");

                entity.Property(e => e.PesServiceProvided).HasComment("个人提供的服务");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .HasComment("电话  ");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .HasComment("备注");

                entity.Property(e => e.ScopeBusiness).HasComment("经营范围");

                entity.Property(e => e.ServiceProvided).HasComment("提供的服务");

                entity.Property(e => e.Sex)
                    .HasMaxLength(100)
                    .HasComment("性别  1,男2,女,3未知");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasComment("状态");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户UUID");

                entity.Property(e => e.TencentQq)
                    .HasColumnName("TencentQQ")
                    .HasMaxLength(100)
                    .HasComment("QQ");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasComment("头衔");

                entity.Property(e => e.WeChat)
                    .HasMaxLength(100)
                    .HasComment("微信");

                entity.Property(e => e.ZjPhone)
                    .HasColumnName("zjPhone")
                    .HasMaxLength(100)
                    .HasComment("座机号码");

                entity.HasOne(d => d.ClientUu)
                    .WithMany(p => p.ContactPerson)
                    .HasForeignKey(d => d.ClientUuid)
                    .HasConstraintName("ContactPerson_FK");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.ContactPerson)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("SystemUser_FK");
            });

            modelBuilder.Entity<ContactPersonView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContactPersonView");

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.BgPhone)
                    .HasColumnName("bgPhone")
                    .HasMaxLength(100);

                entity.Property(e => e.Call).HasMaxLength(100);

                entity.Property(e => e.Cellphone).HasMaxLength(100);

                entity.Property(e => e.ClientManager).HasMaxLength(255);

                entity.Property(e => e.ClientName).HasMaxLength(255);

                entity.Property(e => e.ClientPhone).HasMaxLength(100);

                entity.Property(e => e.ClientUuid).HasColumnName("ClientUUID");

                entity.Property(e => e.ContactName).HasMaxLength(100);

                entity.Property(e => e.ContactPersonUuid).HasColumnName("ContactPersonUUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.RealName).HasMaxLength(255);

                entity.Property(e => e.Remarks).HasMaxLength(255);

                entity.Property(e => e.Sex).HasMaxLength(100);

                entity.Property(e => e.TencentQq)
                    .HasColumnName("TencentQQ")
                    .HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.WeChat).HasMaxLength(100);

                entity.Property(e => e.ZjPhone)
                    .HasColumnName("zjPhone")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ContactPlist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContactPList");

                entity.Property(e => e.ContactName).HasMaxLength(357);

                entity.Property(e => e.ContactPersonUuid).HasColumnName("ContactPersonUUID");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.ClientUuid)
                    .HasName("PK_Client")
                    .IsClustered(false);

                entity.HasComment("客户表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Customer")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ClientUuid)
                    .HasColumnName("ClientUUID")
                    .HasComment("客户UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.BusinessUuid)
                    .HasColumnName("BusinessUUID")
                    .HasComment("商机UUID");

                entity.Property(e => e.CallLogUuid)
                    .HasColumnName("CallLogUUID")
                    .HasComment("联系记录UUID");

                entity.Property(e => e.ClientAddress).HasComment("地址");

                entity.Property(e => e.ClientArea)
                    .HasMaxLength(255)
                    .HasComment("地区");

                entity.Property(e => e.ClientBirthday)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("客户生日");

                entity.Property(e => e.ClientEmail)
                    .HasMaxLength(255)
                    .HasComment("邮件");

                entity.Property(e => e.ClientFax)
                    .HasMaxLength(255)
                    .HasComment("传真");

                entity.Property(e => e.ClientIndustry).HasComment("行业");

                entity.Property(e => e.ClientManager).HasComment("客户经理");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(255)
                    .HasComment("客户名称");

                entity.Property(e => e.ClientPhone)
                    .HasMaxLength(100)
                    .HasComment("电话");

                entity.Property(e => e.ClientPostcode)
                    .HasMaxLength(255)
                    .HasComment("邮编");

                entity.Property(e => e.ClientStatus).HasComment("客户状态");

                entity.Property(e => e.ClientType).HasComment("类型");

                entity.Property(e => e.ClientWebsite).HasComment("网站");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否删除  0未删   1已删");

                entity.Property(e => e.Remarks).HasComment("备注");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasComment("状态");

                entity.Property(e => e.SuperiorMenu).HasComment("客户级别 0一级");

                entity.Property(e => e.SuperiorUuid)
                    .HasColumnName("SuperiorUUID")
                    .HasComment("上级客户");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户UUID");

                entity.HasOne(d => d.ClientIndustryNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ClientIndustry)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Client_Industry");

                entity.HasOne(d => d.ClientManagerNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ClientManager)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Client_SystemUser");

                entity.HasOne(d => d.ClientStatusNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ClientStatus)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Client_Client");

                entity.HasOne(d => d.ClientTypeNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ClientType)
                    .HasConstraintName("FK_Client_TypeList");
            });

            modelBuilder.Entity<CustomerIndustry>(entity =>
            {
                entity.HasKey(e => e.IndustryUuid)
                    .HasName("PK_Industry");

                entity.HasComment("行业表");

                entity.Property(e => e.IndustryUuid)
                    .HasColumnName("IndustryUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IndustryName).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerStatus>(entity =>
            {
                entity.HasKey(e => e.CustomerStatusUuid);

                entity.HasComment("状态表");

                entity.Property(e => e.CustomerStatusUuid)
                    .HasColumnName("CustomerStatusUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerTree>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CustomerTree");

                entity.Property(e => e.BusinessUuid).HasColumnName("BusinessUUID");

                entity.Property(e => e.ClientArea).HasMaxLength(255);

                entity.Property(e => e.ClientBirthday)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName).HasMaxLength(255);

                entity.Property(e => e.ClientUuid).HasColumnName("ClientUUID");

                entity.Property(e => e.CpName).HasColumnName("cpName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IndustryName).HasMaxLength(50);

                entity.Property(e => e.RealName).HasMaxLength(255);

                entity.Property(e => e.StatusName).HasMaxLength(50);

                entity.Property(e => e.SuperiorUuid).HasColumnName("SuperiorUUID");

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");
            });

            modelBuilder.Entity<CustomerTypeList>(entity =>
            {
                entity.HasKey(e => e.TypeUuid)
                    .HasName("TypeList_PK");

                entity.HasComment("客户类型表");

                entity.Property(e => e.TypeUuid)
                    .HasColumnName("TypeUUID")
                    .HasComment("类型UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("类型名称");
            });

            modelBuilder.Entity<SystemDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentUuid)
                    .HasName("PK__Departme__1AA1F4C05ED85AB0");

                entity.HasComment("部门");

                entity.Property(e => e.DepartmentUuid)
                    .HasColumnName("DepartmentUUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Dingid).HasComment("钉钉部门id");

                entity.Property(e => e.EstablishName)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.EstablishTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDeleted).HasComment("是否删除（0.未删除，1已删除）");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasComment("名称");

                entity.Property(e => e.Remark)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("备注");
            });

            modelBuilder.Entity<SystemIcon>(entity =>
            {
                entity.HasKey(e => e.SystemIconUuid)
                    .HasName("PK__SystemIc__540CED9D42DF8109");

                entity.HasComment("图标");

                entity.Property(e => e.SystemIconUuid)
                    .HasColumnName("SystemIconUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Custom).HasMaxLength(60);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size).HasMaxLength(20);
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.SystemLogUuid)
                    .HasName("PK__SystemLo__65A475E79A921FFD");

                entity.HasComment("系统日志表");

                entity.Property(e => e.SystemLogUuid)
                    .HasColumnName("SystemLogUUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("IP地址");

                entity.Property(e => e.IsDelete).HasComment("标记删除 0，未删除1，已删除");

                entity.Property(e => e.OperationContent)
                    .IsRequired()
                    .HasComment("操作内容");

                entity.Property(e => e.OperationTime).HasComment("操作时间");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作类型(增删改查)");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("操作用户ID");

                entity.Property(e => e.UserIdtype)
                    .HasColumnName("UserIDType")
                    .HasComment("用户名和类型");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作用户用户名");
            });

            modelBuilder.Entity<SystemMenu>(entity =>
            {
                entity.HasKey(e => e.SystemMenuUuid)
                    .HasName("PK__DncMenu__A2B5777CA1511602");

                entity.HasComment("菜单表");

                entity.Property(e => e.SystemMenuUuid)
                    .HasColumnName("SystemMenuUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemMessage>(entity =>
            {
                entity.HasKey(e => e.MessageUuid)
                    .HasName("PK_Message");

                entity.HasComment("信息表");

                entity.Property(e => e.MessageUuid)
                    .HasColumnName("MessageUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.ClientUuid).HasColumnName("ClientUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.ClientUu)
                    .WithMany(p => p.SystemMessage)
                    .HasForeignKey(d => d.ClientUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Message_FK");
            });

            modelBuilder.Entity<SystemPermission>(entity =>
            {
                entity.HasKey(e => e.SystemPermissionUuid)
                    .HasName("PK__DncPermi__18DD8CCDCC3FD718");

                entity.HasComment("权限表");

                entity.Property(e => e.SystemPermissionUuid)
                    .HasColumnName("SystemPermissionUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionCode).HasMaxLength(255);

                entity.Property(e => e.CaPower).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.HasOne(d => d.SystemMenuUu)
                    .WithMany(p => p.SystemPermission)
                    .HasForeignKey(d => d.SystemMenuUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SystemPermission_SystemMenu");
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.HasKey(e => e.SystemRoleUuid)
                    .HasName("PK__DncRole__DF75FB283AD1E2C6");

                entity.HasComment("角色表");

                entity.HasIndex(e => e.RoleName)
                    .HasName("UQ_roleName")
                    .IsUnique();

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsFixation)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否内置  0否,1是");

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemRolePermissionMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemRoleUuid, e.SystemPermissionUuid })
                    .HasName("PK__DncRoleP__1EF823E41817FDB5");

                entity.HasComment("角色权限关系");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.SystemPermissionUuid).HasColumnName("SystemPermissionUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasComment("全局设置");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime).HasComment("添加时间");

                entity.Property(e => e.AppPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.ClobalName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("全局配置名称");

                entity.Property(e => e.GlobalLogo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("全局配置logo");

                entity.Property(e => e.Globalurl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("全局配置图标路径");

                entity.Property(e => e.IsDelete)
                    .HasColumnName("isDelete")
                    .HasComment("是否删除");
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasKey(e => e.SystemUserUuid)
                    .HasName("PK__DncUser__A2B5777C0780DFF0")
                    .IsClustered(false);

                entity.HasComment("系统用户");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("注册时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("地址");

                entity.Property(e => e.CompanyUuid)
                    .HasColumnName("CompanyUUID")
                    .HasComment("公司UUID");

                entity.Property(e => e.DepartmentUuid).HasColumnName("DepartmentUUID");

                entity.Property(e => e.Ewm)
                    .HasColumnName("EWM")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("二维码");

                entity.Property(e => e.GrabageRoomId)
                    .HasColumnName("GrabageRoomID")
                    .HasComment("垃圾厢房id");

                entity.Property(e => e.HomeAddressUuid)
                    .HasColumnName("HomeAddressUUID")
                    .HasComment("家庭码UUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdcardMd5)
                    .HasColumnName("IDCardMD5")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("身份证号MD5");

                entity.Property(e => e.InTime)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("入职时间");

                entity.Property(e => e.IsDeleted).HasComment("0正常 1删除");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(255)
                    .HasComment("登录名");

                entity.Property(e => e.ManageDepartment).HasColumnType("text");

                entity.Property(e => e.OldCard)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("邮箱");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(255)
                    .HasComment("密码");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("电话");

                entity.Property(e => e.ProblemContent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("反馈内容");

                entity.Property(e => e.ProblemType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("问题类型");

                entity.Property(e => e.RealName)
                    .HasMaxLength(255)
                    .HasComment("真实姓名");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("备注");

                entity.Property(e => e.Sex)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("性别");

                entity.Property(e => e.ShopUuid).HasColumnName("ShopUUID");

                entity.Property(e => e.Streets)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("钉钉 userId");

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUid")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("角色id,用逗号分隔");

                entity.Property(e => e.Types)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("部门");

                entity.Property(e => e.UserIdCard)
                    .HasMaxLength(255)
                    .HasComment("身份证");

                entity.Property(e => e.UserType).HasComment("1 超管 2其他");

                entity.Property(e => e.VillageId)
                    .HasColumnName("VillageID")
                    .HasComment("社区id");

                entity.Property(e => e.Wechat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("微信");

                entity.Property(e => e.ZaiGang)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("状态");

                entity.HasOne(d => d.DepartmentUu)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.DepartmentUuid)
                    .HasConstraintName("Department_FK");
            });

            modelBuilder.Entity<SystemUserRoleMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemUserUuid, e.SystemRoleUuid })
                    .HasName("PK__DncUserR__328A96E56EE320C2");

                entity.HasComment("用户角色关系");

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ViewMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Menu");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.Pt).HasColumnName("pt");

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu2");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
