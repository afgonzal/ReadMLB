using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadMLB2020;

namespace ReadMLB.Services
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            DataLayer.Startup.ConfigureServices(services, config);
            services.AddScoped<ITeamsService, TeamsService>();
            services.AddScoped<IPlayersService, PlayersService>();
            services.AddScoped<IBattingService, BattingService>();
            services.AddScoped<IPitchingService, PitchingService>();
            services.AddScoped<IRostersService, RostersService>();
            services.AddScoped<IRunningStatsService, RunningStatsService>();
            services.AddScoped<IDefenseStatsService, DefenseStatsService>();
            services.AddScoped<IRotationsService, RotationsService>();
            services.AddScoped<IScheduleService, ScheduleService>();

        }
    }
}
