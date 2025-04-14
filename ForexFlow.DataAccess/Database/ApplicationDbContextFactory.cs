using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ForexFlow.DataAccess.Database
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            string exePath = AppContext.BaseDirectory;
            string solutionRootDir = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\.."));

            var databasePath = Path.Combine(solutionRootDir, "ForexFlow.DataAccess", "Database", "currency_system.db");

            optionsBuilder.UseSqlite($"Data Source={databasePath}");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
