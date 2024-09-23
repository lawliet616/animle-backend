using Quartz;
using System.Runtime.CompilerServices;

namespace Animle.services.Quartz
{
    public static class Quartz
    {
        public static void RegisterCronJobs(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var monthly = JobKey.Create("Monthly");
                var daily = JobKey.Create("Daily");
                var weekly = JobKey.Create("Weekly");
                var instant = JobKey.Create("Instant");
                options.UseMicrosoftDependencyInjectionJobFactory();
                options.AddJob<MonthlyJob>(monthly).AddTrigger(trigger =>
                {
                    trigger.ForJob(monthly).WithCronSchedule("* * 1 * * ?");
                });
                options.AddJob<MonthlyJob>(instant).AddTrigger(trigger =>
                {
                    trigger.ForJob(instant).StartNow();
                });
                options.AddJob<DailyJob>(daily).AddTrigger(trigger =>
                {
                    trigger.ForJob(daily).WithCronSchedule("0 1 * * * ?");
                });
                options.AddJob<WeeklyJob>(weekly).AddTrigger(trigger =>
                {
                    trigger.ForJob(weekly).WithCronSchedule("* * * * 1 ?");
                });
            });
            services.AddQuartzHostedService();
        }

    }
}
