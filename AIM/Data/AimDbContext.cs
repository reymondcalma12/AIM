using Microsoft.EntityFrameworkCore;
using AIM.Models;
namespace AIM.Data
{
    using AIM.Models;
    using Microsoft.EntityFrameworkCore;

    public class AimDbContext : DbContext
    {
        public AimDbContext(DbContextOptions<AimDbContext> options) : base(options)
        {
        }


        public DbSet<User> tbl_aim_users { get; set; }
        public DbSet<Profile> tbl_aim_profiles { get; set; }
        public DbSet<Module> tbl_aim_modules { get; set; }
        public DbSet<Category> tbl_aim_category { get; set; }
        public DbSet<Status> tbl_aim_status { get; set; }
        public DbSet<Parameter> tbl_aim_parameters { get; set; }
        public DbSet<ProfileAccess> tbl_aim_profileaccess { get; set; }
        public DbSet<Store> tbl_aim_stores { get; set; }
        public DbSet<Application> tbl_aim_applications { get; set; }

        public DbSet<Area> tbl_aim_area { get; set; }

        public DbSet<AppAreaAffected> tbl_aim_app_areaaffected { get; set; }

        public DbSet<AppBusinessImpact> tbl_aim_appbusimpact { get; set; }

        public DbSet<AppLocation> tbl_aim_apploc { get; set; }

        public DbSet<AppSystemAffected> tbl_aim_app_systemaffected { get; set; }

        public DbSet<Department> tbl_aim_department { get; set; }

        public DbSet<AppDependency> tbl_aim_dependencies { get; set; }

        public DbSet<DepartmentProcessOwner> tbl_aim_deptprocessowner { get; set; }

        public DbSet<Level3> tbl_aim_level3 { get; set; }

        public DbSet<Level3Owner> tbl_aim_level3owner { get; set; }


        public DbSet<RPortOsla> tbl_aim_rportosla { get; set; }

        public DbSet<SLALevel> tbl_aim_slalevel { get; set; }

        public DbSet<BusinessImpact> tbl_aim_businessimpact { get; set; }

        public DbSet<AuthMethod> tbl_aim_authmethod { get; set; }
        public DbSet<BrowserCompatibility> tbl_aim_app_browsercompatibility { get; set; }
        public DbSet<CriticalLevel> tbl_aim_criticallevel { get; set; }
        public DbSet<FunctionalArea> tbl_aim_functionalarea { get; set; }
        public DbSet<Level2> tbl_aim_level2 { get; set; }
        public DbSet<OperatingSystem> tbl_aim_operatingsystem { get; set; }
        public DbSet<PrintSpooler> tbl_aim_printspooler { get; set; }
        public DbSet<SystemClass> tbl_aim_systemclass { get; set; }
        public DbSet<SupportType> tbl_aim_supporttype { get; set; } // Added SupportType DbSet
        public DbSet<AppContact> tbl_aim_app_contacts { get; set; } // Added AppContact DbSet
        public DbSet<AppDeptProcOwner> tbl_aim_app_deptprocowner { get; set; } // Added AppDeptProcOwner DbSet
        public DbSet<AppFunctionalAreaOwner> tbl_aim_app_functionalarea_owner { get; set; } // Added AppFunctionalAreaOwner DbSet
        public DbSet<AppIPAddress> tbl_aim_app_ipaddress { get; set; } // Added AppIPAddress DbSet

        public DbSet<AppModuleDependency> tbl_aim_app_moduledependencies { get; set; } // Added DbSet for AppModuleDependency
        public DbSet<AppSupport> tbl_aim_app_support { get; set; } // Added DbSet for AppSupport
        public DbSet<AppCategory> tbl_aim_appcategory { get; set; } // Added DbSet for AppCategory
        public DbSet<Group> tbl_aim_group { get; set; } // Added DbSet for Group
        public DbSet<Server> tbl_aim_server { get; set; } // Added DbSet for Server
        public DbSet<Browser> tbl_aim_browser { get; set; } // Added DbSet for Server
        public DbSet<ServerType> tbl_aim_servertype { get; set; } // Added DbSet for Support


        public DbSet<Support> tbl_aim_supports { get; set; } // Added DbSet for Support

        public DbSet<tbl_aim_subscriptionType> tbl_aim_subscriptionType { get; set; } // Added DbSet for Support


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<tbl_aim_subscriptionType>().HasKey(s => s.subscriptionType_code);

            // Support
            modelBuilder.Entity<Support>().HasKey(s => s.SupportCode);
            modelBuilder.Entity<Support>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.SupportStatus);
            modelBuilder.Entity<Support>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.SupportCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<Support>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.SupportUpdatedBy);


            // Server
            modelBuilder.Entity<Server>().HasKey(s => s.ServerCode);
            modelBuilder.Entity<Server>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.ServerStatus);
            modelBuilder.Entity<Server>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.ServerCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<Server>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.ServerUpdatedBy);

            // Group
            modelBuilder.Entity<Group>().HasKey(g => g.GroupCode);
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Status)
                .WithMany()
                .HasForeignKey(g => g.GroupStatus);
            modelBuilder.Entity<Group>()
                .HasOne(g => g.CreatedBy)
                .WithMany()
                .HasForeignKey(g => g.GroupCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<Group>()
                .HasOne(g => g.UpdatedBy)
                .WithMany()
                .HasForeignKey(g => g.GroupUpdatedBy);

            // AppCategory
            modelBuilder.Entity<AppCategory>().HasKey(c => c.CatCode);
            modelBuilder.Entity<AppCategory>()
                .HasOne(c => c.Status)
                .WithMany()
                .HasForeignKey(c => c.CatStatus);
            modelBuilder.Entity<AppCategory>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CatCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<AppCategory>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.CatUpdatedBy);

            // ServerType
            modelBuilder.Entity<ServerType>().HasKey(e => e.TypeCode);
            modelBuilder.Entity<ServerType>()
                .HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(e => e.TypeStatus);
            modelBuilder.Entity<ServerType>()
                .HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.TypeCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<ServerType>()
                .HasOne(e => e.UpdatedBy)
                .WithMany()
                .HasForeignKey(e => e.TypeUpdatedBy);

            // AppSupport
            modelBuilder.Entity<AppSupport>().HasKey(s => new { s.AppCode, s.SupportCode, s.SupportTypeCode });
            modelBuilder.Entity<AppSupport>()
                .HasOne(s => s.Application)
                .WithMany()
                .HasForeignKey(s => s.AppCode);
            modelBuilder.Entity<AppSupport>()
                .HasOne(s => s.Support)
                .WithMany()
                .HasForeignKey(s => s.SupportCode);
            modelBuilder.Entity<AppSupport>()
                .HasOne(s => s.SupportType)
                .WithMany()
                .HasForeignKey(s => s.SupportTypeCode);
            modelBuilder.Entity<AppSupport>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy);

            // AppModuleDependency
            modelBuilder.Entity<AppModuleDependency>()
                 .HasKey(d => new { d.AppCode, d.ModuleCode });

            modelBuilder.Entity<AppModuleDependency>()
                .HasOne(d => d.Application)
                .WithMany()
                .HasForeignKey(d => d.AppCode)
                .HasPrincipalKey(a => a.AppCode); // Specify principal key

            modelBuilder.Entity<AppModuleDependency>()
                .HasOne(d => d.DependencyApplication)
                .WithMany()
                .HasForeignKey(d => d.ModuleCode)
                .HasPrincipalKey(a => a.AppCode); // Specify principal key

            modelBuilder.Entity<AppModuleDependency>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy);

            // AppIPAddress
            modelBuilder.Entity<AppIPAddress>().HasKey(a => new { a.AppCode, a.IPAddress });
            modelBuilder.Entity<AppIPAddress>()
                .HasOne(a => a.Application)
                .WithMany()
                .HasForeignKey(a => a.AppCode);
            modelBuilder.Entity<AppIPAddress>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.CreatedBy);


            // AppFunctionalAreaOwner
            modelBuilder.Entity<AppFunctionalAreaOwner>().HasKey(afa => new { afa.AppCode, afa.FunctionalCode });
            modelBuilder.Entity<AppFunctionalAreaOwner>()
                .HasOne(afa => afa.Application)
                .WithMany()
                .HasForeignKey(afa => afa.AppCode);
            modelBuilder.Entity<AppFunctionalAreaOwner>()
                .HasOne(afa => afa.FunctionalArea)
                .WithMany()
                .HasForeignKey(afa => afa.FunctionalCode);
            modelBuilder.Entity<AppFunctionalAreaOwner>()
                .HasOne(afa => afa.User)
                .WithMany()
                .HasForeignKey(afa => afa.CreatedBy);

            // AppDeptProcOwner
            modelBuilder.Entity<AppDeptProcOwner>().HasKey(adpo => new { adpo.AppCode, adpo.DeptCode });
            modelBuilder.Entity<AppDeptProcOwner>()
                .HasOne(adpo => adpo.Application)
                .WithMany()
                .HasForeignKey(adpo => adpo.AppCode);
            modelBuilder.Entity<AppDeptProcOwner>()
                .HasOne(adpo => adpo.Department)
                .WithMany()
                .HasForeignKey(adpo => adpo.DeptCode);
            modelBuilder.Entity<AppDeptProcOwner>()
                .HasOne(adpo => adpo.User)
                .WithMany()
                .HasForeignKey(adpo => adpo.CreatedBy);


            // SupportType
            modelBuilder.Entity<SupportType>().HasKey(s => s.SupportTypeCode);
            modelBuilder.Entity<SupportType>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.SupportTypeStatus);
            modelBuilder.Entity<SupportType>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.SupportTypeCreatedBy);
            modelBuilder.Entity<SupportType>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.SupportTypeUpdatedBy);

            // AppContact
            modelBuilder.Entity<AppContact>().HasKey(ac => new { ac.AppCode, ac.ContactNo, ac.SupportTypeCode });
            modelBuilder.Entity<AppContact>()
                .HasOne(ac => ac.Application)
                .WithMany()
                .HasForeignKey(ac => ac.AppCode);
            modelBuilder.Entity<AppContact>()
                .HasOne(ac => ac.SupportType)
                .WithMany()
                .HasForeignKey(ac => ac.SupportTypeCode);
            modelBuilder.Entity<AppContact>()
                .HasOne(ac => ac.User)
                .WithMany()
                .HasForeignKey(ac => ac.CreatedBy);


            //SystemClass
            modelBuilder.Entity<SystemClass>().HasKey(c => c.ClassCode);
            modelBuilder.Entity<SystemClass>()
                .HasOne(c => c.Status)
                .WithMany()
                .HasForeignKey(c => c.ClassStatus);
            modelBuilder.Entity<SystemClass>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.ClassCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<SystemClass>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.ClassUpdatedBy);

            //PrintSPooler
            modelBuilder.Entity<PrintSpooler>().HasKey(s => s.SpoolerCode);
            modelBuilder.Entity<PrintSpooler>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.SpoolerStatus);
            modelBuilder.Entity<PrintSpooler>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.SpoolerCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<PrintSpooler>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.SpoolerUpdatedBy);

            //OperatingSystem
            modelBuilder.Entity<OperatingSystem>().HasKey(os => os.OsCode);
            modelBuilder.Entity<OperatingSystem>()
                .HasOne(os => os.Status)
                .WithMany()
                .HasForeignKey(os => os.OsStatus);
            modelBuilder.Entity<OperatingSystem>()
                .HasOne(os => os.CreatedBy)
                .WithMany()
                .HasForeignKey(os => os.OsCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<OperatingSystem>()
                .HasOne(os => os.UpdatedBy)
                .WithMany()
                .HasForeignKey(os => os.OsUpdatedBy);

            //Level2
            modelBuilder.Entity<Level2>().HasKey(l => l.Level2Code);
            modelBuilder.Entity<Level2>()
                .HasOne(l => l.Status)
                .WithMany()
                .HasForeignKey(l => l.Level2Status);
            modelBuilder.Entity<Level2>()
                .HasOne(l => l.CreatedBy)
                .WithMany()
                .HasForeignKey(l => l.Level2CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<Level2>()
                .HasOne(l => l.UpdatedBy)
                .WithMany()
                .HasForeignKey(l => l.Level2UpdatedBy);

            //FunctionalArea
            modelBuilder.Entity<FunctionalArea>().HasKey(f => f.FunctionalCode);
            modelBuilder.Entity<FunctionalArea>()
                .HasOne(f => f.Status)
                .WithMany()
                .HasForeignKey(f => f.FunctionalStatus);
            modelBuilder.Entity<FunctionalArea>()
                .HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(f => f.FunctionalCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<FunctionalArea>()
                .HasOne(f => f.UpdatedBy)
                .WithMany()
                .HasForeignKey(f => f.FunctionalUpdatedBy);

            // CriticalLevel
            modelBuilder.Entity<CriticalLevel>().HasKey(c => c.CriticalLevelCode);
            modelBuilder.Entity<CriticalLevel>()
                .HasOne(c => c.Status)
                .WithMany()
                .HasForeignKey(c => c.CriticalLevelStatus);
            modelBuilder.Entity<CriticalLevel>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CriticalLevelCreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Optional: This prevents cascade delete if needed
            modelBuilder.Entity<CriticalLevel>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.CriticalLevelUpdatedBy);

            // BrowserCompatibility

            modelBuilder.Entity<BrowserCompatibility>()
                .HasKey(bc => new { bc.AppCode, bc.BrowserCode });

            modelBuilder.Entity<BrowserCompatibility>()
         .HasOne(bc => bc.Application)
         .WithMany() // Since there's no direct navigation property, use WithMany() without specifying a property
         .HasForeignKey(bc => bc.AppCode);


            modelBuilder.Entity<BrowserCompatibility>()
                .HasOne(bc => bc.CreatedUser)
                .WithMany()
                .HasForeignKey(bc => bc.CreatedBy);

            modelBuilder.Entity<BrowserCompatibility>()
                .HasOne(bc => bc.Browser)
                .WithMany()
                .HasForeignKey(bc => bc.BrowserCode);


            // AuthMethod
            modelBuilder.Entity<AuthMethod>().HasKey(a => a.AuthCode);
            modelBuilder.Entity<AuthMethod>()
                .HasOne(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.AuthStatus);
            modelBuilder.Entity<AuthMethod>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.AuthCreatedBy);
            modelBuilder.Entity<AuthMethod>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.AuthUpdatedBy);

            // User
            modelBuilder.Entity<User>().HasKey(u => u.UserCode);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Status)
                .WithMany()
                .HasForeignKey(u => u.UserStatus);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithMany()
                .HasForeignKey(u => u.UserProfile);

            // Profile
            modelBuilder.Entity<Profile>().HasKey(p => p.ProfileId);
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.ProfileStatus);
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.ProfileCreated);
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.ProfileUpdated);

            // Status
            modelBuilder.Entity<Status>().HasKey(s => s.StatusCode);

            // Module
            modelBuilder.Entity<Module>().HasKey(m => m.ModuleId);
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Category)
                .WithMany()
                .HasForeignKey(m => m.ModuleCategory);
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Status)
                .WithMany()
                .HasForeignKey(m => m.ModuleStatus);
            modelBuilder.Entity<Module>()
                .HasOne(m => m.CreatedBy)
                .WithMany()
                .HasForeignKey(m => m.ModuleCreated);
            modelBuilder.Entity<Module>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.ModuleUpdated);

            // Category
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>()
             .HasOne(m => m.Status)
             .WithMany()
             .HasForeignKey(m => m.CategoryStatus);

            // Parameter
            modelBuilder.Entity<Parameter>().HasKey(p => p.ParmCode);

            // ProfileAccess
            modelBuilder.Entity<ProfileAccess>().HasKey(pa => new { pa.ProfileId, pa.ModuleId });
            modelBuilder.Entity<ProfileAccess>()
                .HasOne(pa => pa.Module)
                .WithMany()
                .HasForeignKey(pa => pa.ModuleId);
            modelBuilder.Entity<ProfileAccess>()
                .HasOne(pa => pa.Profile)
                .WithMany()
                .HasForeignKey(pa => pa.ProfileId);
            modelBuilder.Entity<ProfileAccess>()
                .HasOne(pa => pa.CreatedBy)
                .WithMany()
                .HasForeignKey(pa => pa.UserCreated);
            modelBuilder.Entity<ProfileAccess>()
                .HasOne(pa => pa.UpdatedBy)
                .WithMany()
                .HasForeignKey(pa => pa.UserUpdated);

            // Store
            modelBuilder.Entity<Store>().HasKey(s => s.StoreCode);
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.StoreStatus);


            // Application
            modelBuilder.Entity<Application>()
                 .HasKey(a => a.AppCode);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.AppCategory);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.CriticalLevel)
                .WithMany()
                .HasForeignKey(a => a.AppCritLevel);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.SLALevel)
                .WithMany()
                .HasForeignKey(a => a.AppSLALevel);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Location)
                .WithMany()
                .HasForeignKey(a => a.AppLocation);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Server)
                .WithMany()
                .HasForeignKey(a => a.AppServerName);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.OS)
                .WithMany()
                .HasForeignKey(a => a.AppOS);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.AuthMethod)
                .WithMany()
                .HasForeignKey(a => a.AppAuthMethod);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.PrintSpooler)
                .WithMany()
                .HasForeignKey(a => a.AppPrintSpool);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Group)
                .WithMany()
                .HasForeignKey(a => a.AppGroupName);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.SystemClass)
                .WithMany()
                .HasForeignKey(a => a.AppSystemClass);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.AppStatus);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.AppCreatedBy);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.AppUpdatedBy);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.tbl_aim_subscriptionType)
                .WithMany()
                .HasForeignKey(a => a.subscriptionType_code);


            // Area
            modelBuilder.Entity<Area>().HasKey(a => a.AreaCode);
            modelBuilder.Entity<Area>()
                .HasOne(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.AreaStatus);
            modelBuilder.Entity<Area>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.AreaCreatedBy);
            modelBuilder.Entity<Area>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.AreaUpdatedBy);

            // AppAreaAffected
            modelBuilder.Entity<AppAreaAffected>().HasKey(a => new { a.AppCode, a.AreaCode });

            modelBuilder.Entity<AppAreaAffected>()
                .HasOne(a => a.Application)
                .WithMany()
                .HasForeignKey(a => a.AppCode)
                .HasPrincipalKey(app => app.AppCode);

            modelBuilder.Entity<AppAreaAffected>()
                .HasOne(a => a.Area)
                .WithMany()
                .HasForeignKey(a => a.AreaCode)
                .HasPrincipalKey(area => area.AreaCode);

            modelBuilder.Entity<AppAreaAffected>()
                .HasOne(a => a.CreatedByUser)
                .WithMany()
                .HasForeignKey(a => a.CreatedBy)
                .HasPrincipalKey(user => user.UserCode);



            // AppBusinessImpact
            modelBuilder.Entity<AppBusinessImpact>().HasKey(a => new { a.RPortOslaCode, a.ImpactCode });
            modelBuilder.Entity<AppBusinessImpact>()
                .HasOne(a => a.RPortOsla)
                .WithMany()
                .HasForeignKey(a => a.RPortOslaCode);
            modelBuilder.Entity<AppBusinessImpact>()
                .HasOne(a => a.BusinessImpact)
                .WithMany()
                .HasForeignKey(a => a.ImpactCode);
            modelBuilder.Entity<AppBusinessImpact>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.ImpactCreatedBy);

            // AppLocation
            modelBuilder.Entity<AppLocation>().HasKey(a => a.LocationCode);
            modelBuilder.Entity<AppLocation>()
                .HasOne(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.LocationStatus);
            modelBuilder.Entity<AppLocation>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.LocationCreatedBy);
            modelBuilder.Entity<AppLocation>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.LocationUpdatedBy);

            // AppSystemsAffected
            modelBuilder.Entity<AppSystemAffected>()
                .HasKey(s => new { s.AppCode, s.SystemCode });

            modelBuilder.Entity<AppSystemAffected>()
                .HasOne(s => s.Application)
                .WithMany()
                .HasForeignKey(s => s.AppCode)
                .HasPrincipalKey(a => a.AppCode);

            modelBuilder.Entity<AppSystemAffected>()
                .HasOne(s => s.System)
                .WithMany()
                .HasForeignKey(s => s.SystemCode)  // Corrected from s.AppCode to s.SystemCode
                .HasPrincipalKey(a => a.AppCode);  // Assuming it's still related to the AppCode property of the Application entity

            modelBuilder.Entity<AppSystemAffected>()
                .HasOne(s => s.CreatedByUser)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy);



            // Department
            modelBuilder.Entity<Department>().HasKey(d => d.DeptCode);
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.DeptStatus);
            modelBuilder.Entity<Department>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.DeptCreatedBy);
            modelBuilder.Entity<Department>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.DeptUpdatedBy);

            // AppDependency
            modelBuilder.Entity<AppDependency>().HasKey(d => new { d.RPortOslaCode, d.AppCode });
            modelBuilder.Entity<AppDependency>()
                .HasOne(d => d.RPortOsla)
                .WithMany()
                .HasForeignKey(d => d.RPortOslaCode);
            modelBuilder.Entity<AppDependency>()
                .HasOne(d => d.Application)
                .WithMany()
                .HasForeignKey(d => d.AppCode);
            modelBuilder.Entity<AppDependency>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.DependencyCreatedBy);

            // DepartmentProcessOwner
            modelBuilder.Entity<DepartmentProcessOwner>().HasKey(d => new { d.RPortOslaCode, d.DeptCode });
            modelBuilder.Entity<DepartmentProcessOwner>()
                .HasOne(d => d.RPortOsla)
                .WithMany()
                .HasForeignKey(d => d.RPortOslaCode);
            modelBuilder.Entity<DepartmentProcessOwner>()
                .HasOne(d => d.Department)
                .WithMany()
                .HasForeignKey(d => d.DeptCode);
            modelBuilder.Entity<DepartmentProcessOwner>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.DeptProcessOwnerCreatedBy);


            // Level3
            modelBuilder.Entity<Level3>().HasKey(l => l.Level3Code);
            modelBuilder.Entity<Level3>()
                .HasOne(l => l.Status)
                .WithMany()
                .HasForeignKey(l => l.Level3Status);
            modelBuilder.Entity<Level3>()
                .HasOne(l => l.CreatedBy)
                .WithMany()
                .HasForeignKey(l => l.Level3CreatedBy);
            modelBuilder.Entity<Level3>()
                .HasOne(l => l.UpdatedBy)
                .WithMany()
                .HasForeignKey(l => l.Level3UpdatedBy);

            // Level3Owner
            modelBuilder.Entity<Level3Owner>().HasKey(l => new { l.RPortOslaCode, l.Level3Code });
            modelBuilder.Entity<Level3Owner>()
                .HasOne(l => l.RPortOsla)
                .WithMany()
                .HasForeignKey(l => l.RPortOslaCode);
            modelBuilder.Entity<Level3Owner>()
                .HasOne(l => l.Level3)
                .WithMany()
                .HasForeignKey(l => l.Level3Code);
            modelBuilder.Entity<Level3Owner>()
                .HasOne(l => l.CreatedBy)
                .WithMany()
                .HasForeignKey(l => l.Level3OwnerCreatedBy);

            // RPortOsla
            modelBuilder.Entity<RPortOsla>().HasKey(r => r.RPortOslaCode);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.Application)
                .WithMany()
                .HasForeignKey(r => r.AppCode);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.Location)
                .WithMany()
                .HasForeignKey(r => r.AppLocation);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.SLALevel)
                .WithMany()
                .HasForeignKey(r => r.AppSLALevel);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.Status)
                .WithMany()
                .HasForeignKey(r => r.AppStatus);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.AppCreatedBy);
            modelBuilder.Entity<RPortOsla>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(r => r.AppUpdatedBy);

            // SLALevel
            modelBuilder.Entity<SLALevel>().HasKey(s => s.SLALevelCode);
            modelBuilder.Entity<SLALevel>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.SLALevelStatus);
            modelBuilder.Entity<SLALevel>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.SLALevelCreatedBy);
            modelBuilder.Entity<SLALevel>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.SLALevelUpdatedBy);

            // BusinessImpact
            modelBuilder.Entity<BusinessImpact>().HasKey(i => i.ImpactCode);
            modelBuilder.Entity<BusinessImpact>()
                .HasOne(i => i.Status)
                .WithMany()
                .HasForeignKey(i => i.ImpactStatus);
            modelBuilder.Entity<BusinessImpact>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.ImpactCreatedBy);
            modelBuilder.Entity<BusinessImpact>()
                .HasOne(i => i.UpdatedBy)
                .WithMany()
                .HasForeignKey(i => i.ImpactUpdatedBy);
        }








    }
    
}
