using API.Errors;
using Core.Entities.OrderAggregate.Interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions  //56 cleaning up program.cs
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();   //277
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));      //135  Basket
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderService, OrderService>();          //213
            services.AddScoped<IUnitOfWork, UnitOfWork>();          //218
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITokenService, TokenService>();      //173
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //it registers GenericRepository as our service
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //look inside our current domain assembly and register the mapping profiles when the app starts up
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>    //actioncontext as an argument     53
                {
                    var errors = actionContext.ModelState               //access the model states
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>                 //67
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");  //what we allow our browser to access
                });
            });

            return services;
        }
    }
}