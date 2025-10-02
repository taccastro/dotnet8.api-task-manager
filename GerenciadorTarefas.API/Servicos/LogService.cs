using GerenciadorTarefas.API.Modelos.Dados;
using MongoDB.Driver;

namespace GerenciadorTarefas.API.Servicos
{
    public class LogService
    {
        private readonly IMongoCollection<LogEvento>? _logs;
        private readonly ILogger<LogService> _logger;

        public LogService(IConfiguration config, ILogger<LogService> logger)
        {
            _logger = logger;
            try
            {
                var conn = config["MongoDB:ConnectionString"];
                var dbName = config["MongoDB:DatabaseName"];

                if (string.IsNullOrWhiteSpace(conn) || string.IsNullOrWhiteSpace(dbName))
                {
                    _logger.LogWarning("Configuração do MongoDB ausente. Logs serão ignorados.");
                    return;
                }

                var client = new MongoClient(conn);
                var database = client.GetDatabase(dbName);
                _logs = database.GetCollection<LogEvento>("Logs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao inicializar o MongoDB. Logs serão ignorados.");
            }
        }

        public async Task RegistrarAsync(LogEvento log)
        {
            if (_logs == null)
            {
                _logger.LogInformation("[LogService] (sem Mongo) {Acao} - {Usuario} - {Detalhes}", log.Acao, log.Usuario, log.Detalhes);
                return;
            }

            try
            {
                await _logs.InsertOneAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao gravar log no Mongo. Registro será ignorado.");
            }
        }

        public async Task<List<LogEvento>> ListarAsync()
        {
            if (_logs == null)
            {
                _logger.LogWarning("ListarAsync chamado sem Mongo configurado. Retornando lista vazia.");
                return new List<LogEvento>();
            }

            try
            {
                return await _logs.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao listar logs no Mongo. Retornando lista vazia.");
                return new List<LogEvento>();
            }
        }
    }
}
