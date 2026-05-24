using Microsoft.Extensions.Logging;
using TodoAppBL.Interfaces;
using TodoAppBL.Services;
using TodoAppBL.Strategies;
using TodoAppDL;
using TodoAppDL.Repositories;
using TodoApp.Pages;
using TodoApp.ViewModels;

namespace TodoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
				.RegisterServices()
				.RegisterStrategies()
				.RegisterViewModels()
				.RegisterRoutes()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<NavigationService>();

            builder.Services.AddTransient<PersonService>();
            builder.Services.AddTransient<TaskService>();

            builder.Services.AddTransient<IPersonRepo, LiteDbPersonRepository>();
            builder.Services.AddTransient<ITaskRepo, LiteDbTaskRepository>();

            builder.Services.AddSingleton<LiteDatabaseConnection>();

            return builder;
        }

        private static MauiAppBuilder RegisterStrategies(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<ITaskListStrategy, AllTasksStrategy>();
            builder.Services.AddTransient<ITaskListStrategy, UnfinishedTasksStrategy>();
            builder.Services.AddTransient<ITaskListStrategy, MostRecentlyUpdatedTasksStrategy>();

            return builder;
        }

		private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<TaskListViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();

            builder.Services.AddTransient<PersonListViewModel>();
            builder.Services.AddTransient<PersonDetailViewModel>();

            builder.Services.AddTransient<TaskListPage>();
            builder.Services.AddTransient<TaskDetailPage>();

            builder.Services.AddTransient<PersonListPage>();
            builder.Services.AddTransient<PersonDetailPage>();

            return builder;
        }

        private static MauiAppBuilder RegisterRoutes(this MauiAppBuilder builder)
        {
            Routing.RegisterRoute(nameof(TaskDetailPage), typeof(TaskDetailPage));
            Routing.RegisterRoute(nameof(PersonListPage), typeof(PersonListPage));
            Routing.RegisterRoute(nameof(PersonDetailPage), typeof(PersonDetailPage));

            return builder;
        }
    }
}