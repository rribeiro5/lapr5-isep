using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.ConnectionRequests;
using DDDSample1.Infrastructure.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Mission;
using DDDSample1.Domain.Posts;
using DDDSample1.Infrastructure.Missions;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Posts;
using DDDSample1.Infrastructure.Users;

namespace DDDSample1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Reflection to add context
            DbContextAdder.GetDbContextAdder(Configuration.GetConnectionString("DbProviderClassName"))
                .AddDbContext(services, Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();

            ConfigureMyServices(services);

            services.AddCors(opt =>
                            opt.AddPolicy("IT3Client",
                                b => b.WithOrigins("*")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                            ));

            services.AddControllers().AddNewtonsoftJson();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCors("IT3Client");
            
            app.UseHttpsRedirection();

            app.UseRouting();

            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            var friendSuggestionsContainer =
                new FriendSuggestionsContainer(services, Configuration, "FriendSuggestionsAlgorithm");
            
            services.AddTransient<IUnitOfWork,UnitOfWork>();
            
            services.AddTransient<IMissionRepository,MissionRepository>();
            services.AddTransient<MissionService>();

            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IUserNetworkService,UserNetworkService>();
            services.AddTransient<UsersController>();
            
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<ISearchUsersService,SearchUsersService>();

           
            friendSuggestionsContainer.MapService();
            services.AddTransient<SuggestUsersController>();
            
            services.AddTransient<IRequestRepository,RequestRepository>();
            services.AddTransient<IConnectionRequestService,ConnectionRequestService>();

            services.AddTransient<IConnectionRepository,ConnectionRepository>();
            services.AddTransient<IConnectionService,ConnectionService>();
            services.AddTransient<ILeaderboardService,LeaderboardService>();
            services.AddTransient<LeaderboardController>();
            
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<TagCloudController>();

            services.AddTransient<ConnectionsController>();
            services.AddTransient<ConnectionRequestController>();
            services.AddTransient<SearchUsersController>();
            
            services.AddTransient<UserNetworkController>();
            
            services.AddSingleton<IMasterDataPostsHttpClient,MasterDataPostsHttpClient>();
            services.AddSingleton<IArtificialIntelligenceClient,ArtificialIntelligenceClient>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<PostsController>();
        }
    }
}
