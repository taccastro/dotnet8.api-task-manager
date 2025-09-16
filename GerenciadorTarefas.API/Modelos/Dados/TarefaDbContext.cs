using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefas.API.Modelos.Dados
{
    public class TarefaDbContext : DbContext
    {
        public TarefaDbContext(DbContextOptions<TarefaDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } = null!;

    }
}
