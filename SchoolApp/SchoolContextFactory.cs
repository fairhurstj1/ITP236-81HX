//#define Logging
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolModel;

public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("SchoolDb");

        var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
        optionsBuilder.UseSqlServer(connectionString);
#if Logging
        // Add logging
        // ILoggerFactory? loggerFactory = null;
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information)
                .AddConsole(options =>
                {
                    options.FormatterName = ConsoleFormatterNames.Simple;
                })
                .AddSimpleConsole(options =>
                {
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                    options.SingleLine = false;
                    options.TimestampFormat = "hh:mm:ss ";
                });
        });
        return new SchoolContext(optionsBuilder.Options, loggerFactory);
#else 
        return new SchoolContext(optionsBuilder.Options);
#endif
    }
}