using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQLite;

public class DataContext : DataAccess.DataContext<DataContext>
{
    public DataContext(
        DbContextOptions<DataContext> options) : base(options)
    {

    }
}
