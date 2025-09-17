using GerenciadorTarefas.API.Modelos.Dados;
using MongoDB.Driver;

namespace GerenciadorTarefas.API.Servicos
{
    public class LogService
    {
        private readonly IMongoCollection<LogEvento> _logs;

        public LogService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _logs = database.GetCollection<LogEvento>("Logs");
        }

        public async Task RegistrarAsync(LogEvento log) =>
            await _logs.InsertOneAsync(log);

        public async Task<List<LogEvento>> ListarAsync() =>
            await _logs.Find(_ => true).ToListAsync();
    }
}
