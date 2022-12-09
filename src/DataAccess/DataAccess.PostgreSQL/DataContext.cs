using Microsoft.EntityFrameworkCore;

namespace DataAccess.PostgreSQL;

public class DataContext : DataAccess.DataContext<DataContext>
{
    public DataContext(
        DbContextOptions<DataContext> options) : base(options)
    {

    }
}
