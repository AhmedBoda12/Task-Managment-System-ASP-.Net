using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Task_Managment.DAL.Data;
using Task_Managment.DAL.Interfaces;
using Task_Managment.DAL.Repositries;
using Task_Managment.bll.Mapper;
using Task_Managment.bll.Interfaces;
using Task_Managment.bll.Services;



namespace Task_Managment.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        

        public static void AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddHttpContextAccessor();
        }
    }
}
