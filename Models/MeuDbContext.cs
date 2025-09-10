using Microsoft.EntityFrameworkCore;

public class MeuDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use sua string de conexão aqui (exemplo para SQL Server local)
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExemploEFDb;Trusted_Connection=True;");
    }
}
