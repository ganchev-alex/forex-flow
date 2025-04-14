using Microsoft.Extensions.DependencyInjection;
using ForexFlow.DataAccess.Repository;
using System.Windows;
using ForexFlow.ViewModel.ViewModels;
using ForexFlow.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ForexFlow;
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        string exePath = AppContext.BaseDirectory;
        string solutionRootDir = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\.."));

        var databasePath = Path.Combine(solutionRootDir, "ForexFlow.DataAccess", "Database", "currency_system.db");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={databasePath}"));

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddTransient<CurrencyManagementViewModel>();
        services.AddTransient<AmountsManagementViewModel>();
        services.AddTransient<InvoiceManagementViewModel>();
        services.AddTransient<Func<Guid, SingleAmountActionsViewModel>>(provider => (amountId) =>
        {
            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            return new SingleAmountActionsViewModel(unitOfWork, amountId);
        });

        ServiceProvider = services.BuildServiceProvider();

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}

