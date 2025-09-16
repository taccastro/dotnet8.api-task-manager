using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GerenciadorTarefas.API.Servicos
{
    public class AutenticacaoServico
    {
        private readonly TarefaDbContext _db;
        private readonly IConfiguration _config;

        public AutenticacaoServico(TarefaDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // Registrar usuário e salvar no banco
        public UsuarioDto Registrar(RegistroDto dto)
        {
            // Verifica se já existe email
            if (_db.Usuarios.Any(u => u.Email == dto.Email))
                throw new Exception("Email já registrado");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = HashSenha(dto.Senha),
                Role = "User"
            };

            _db.Usuarios.Add(usuario);
            _db.SaveChanges(); // Persiste no banco

            return GerarUsuarioDto(usuario);
        }

        // Login com banco
        public UsuarioDto? Login(LoginDto dto)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

            if (usuario == null || !VerificarSenha(dto.Senha, usuario.SenhaHash))
                return null;

            return GerarUsuarioDto(usuario);
        }

        // Hash da senha
        private string HashSenha(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha.Trim()));
            return Convert.ToBase64String(bytes);
        }

        private bool VerificarSenha(string senha, string hash) =>
            HashSenha(senha) == hash;

        // Geração do DTO com JWT
        private UsuarioDto GerarUsuarioDto(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            });

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
