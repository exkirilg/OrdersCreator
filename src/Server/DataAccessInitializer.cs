using DataAccess;
using Domain.DataAccessInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Server;

/// <summary>
/// 
/// </summary>
public class DataAccessInitializer
{
	private const string BaseDataAccessProviderPath = "DatabaseProvider";

	private const string SQLiteDataAccessProviderPath = "SQLite";
	private const string PostgreSQLDataAccessProviderPath = "PostgreSQL";

	private readonly ConfigurationManager _configuration;

	private readonly bool _sqLite;
	private readonly bool _postgreSQL;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="configuration"></param>
	public DataAccessInitializer(ConfigurationManager configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

		_configuration = configuration;

        _sqLite = bool.Parse(
            _configuration[$"{BaseDataAccessProviderPath}:{SQLiteDataAccessProviderPath}"]!);

        _postgreSQL = bool.Parse(
            _configuration[$"{BaseDataAccessProviderPath}:{PostgreSQLDataAccessProviderPath}"]!);
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public void ConfigureDataAccessServices(IServiceCollection services)
	{
		if (_sqLite)
		{
            services.AddDbContext<DataAccess.SQLite.DataContext>(options =>
            {
                options.UseSqlite(
					$"DataSource={Path.Combine(
						AppContext.BaseDirectory,
                        _configuration.GetConnectionString(SQLiteDataAccessProviderPath)!)}");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork<DataAccess.SQLite.DataContext>>();
        }
		else if (_postgreSQL)
		{
            services.AddDbContext<DataAccess.PostgreSQL.DataContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString(PostgreSQLDataAccessProviderPath));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork<DataAccess.PostgreSQL.DataContext>>();
        }
		else
		{
            throw new Exception("Database provider is not selected");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task EnsureDatabaseCreatedNoPendingMigrations(IServiceProvider serviceProvider)
    {
        if (_sqLite)
        {
            await EnsureDatabaseCreatedNoPendingMigrations<DataAccess.SQLite.DataContext>(serviceProvider);
        }
        else if (_postgreSQL)
        {
            await EnsureDatabaseCreatedNoPendingMigrations<DataAccess.PostgreSQL.DataContext>(serviceProvider);
        }
        else
        {
            throw new Exception("Database provider is not selected");
        }
    }

    private async Task EnsureDatabaseCreatedNoPendingMigrations<TDataContext>(IServiceProvider serviceProvider)
        where TDataContext : DbContext, IDataContext
    {
        await EnsureDatabaseCreated<TDataContext>(serviceProvider);
        await EnsureNoPendingMigrations<TDataContext>(serviceProvider);
    }

    private async Task EnsureDatabaseCreated<TDataContext>(IServiceProvider serviceProvider)
        where TDataContext : DbContext, IDataContext
	{
		using var scope = serviceProvider.CreateScope();

        await scope.ServiceProvider
            .GetRequiredService<TDataContext>()
            .Database.EnsureCreatedAsync();
	}

	private async Task EnsureNoPendingMigrations<TDataContext>(IServiceProvider serviceProvider)
        where TDataContext : DbContext, IDataContext
    {
        using var scope = serviceProvider.CreateScope();

        var pendingMigrations = await scope.ServiceProvider
            .GetRequiredService<TDataContext>().Database.EnsureCreatedAsync();

        if (pendingMigrations)
        {
            await scope.ServiceProvider.GetRequiredService<TDataContext>().Database.MigrateAsync();
        }
    }
}
