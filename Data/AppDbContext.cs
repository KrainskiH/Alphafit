using Microsoft.EntityFrameworkCore;
using AlphaFit.Models;

namespace AlphaFit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Instrutor> Instrutores => Set<Instrutor>();
    public DbSet<Treino> Treinos => Set<Treino>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
}