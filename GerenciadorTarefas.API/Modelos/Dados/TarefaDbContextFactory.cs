using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GerenciadorTarefas.API.Modelos.Dados
{
    public class TarefaDbContextFactory : IDesignTimeDbContextFactory<TarefaDbContext>
    {
        public TarefaDbContext CreateDbContext(string[] args)
        {
            // Ajuste o caminho base para encontrar o appsettings.json
            var basePath = AppContext.BaseDirectory; // diretório onde o EF está rodando
            var projectPath = Path.Combine(basePath, "..", "..", ".."); // sobe até a raiz do projeto

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("BancoPostgreSQL");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("A connection string não foi encontrada! Verifique o appsettings.json.");

            var optionsBuilder = new DbContextOptionsBuilder<TarefaDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TarefaDbContext(optionsBuilder.Options);
        }
    }
}
