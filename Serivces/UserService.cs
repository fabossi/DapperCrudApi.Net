using AutoMapper;
using Dapper;
using DapperCrudApi.Dto;
using DapperCrudApi.Models;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace DapperCrudApi.Serivces
{
    public class UserService : IUserInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResponseModel<List<UserListDto>>> BuscarUsuarios()
        {

            ResponseModel<List<UserListDto>> response = new ResponseModel<List<UserListDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBD = await connection.QueryAsync<UserModel>("select * from Usuarios");

                if (usuariosBD.Count() == 0)
                {
                    response.Mensagem = "Nenhum usuario localizado";
                    response.Status = false;
                    return response;

                }

                var usuarioMapeado = _mapper.Map<List<UserListDto>>(usuariosBD);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários localizados com sucesso";

            }
            return response;

        }
        public async Task<ResponseModel<UserModel>> BuscarUsuarioPorId(int Id)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBD = await connection.QueryFirstOrDefaultAsync<UserModel>("select * from Usuarios where Usuarios.Id= @Id", new { Id });

                if (usuarioBD == null)
                {
                    response.Mensagem = "Nenhum usuário localizado";
                    response.Status = false;
                    return response;
                }

                response.Dados = usuarioBD; 
                response.Mensagem = "Usuário localizado com sucesso";
            }
            return response;
        }
        public async Task<ResponseModel<List<UserListDto>>> CriarUsuario(UserCreationDto userCreationDto)
        {
            ResponseModel<List<UserListDto>> response = new ResponseModel<List<UserListDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                var usuariosBanco = await connection.ExecuteAsync("insert into Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao) " +
                                                                    "values (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", userCreationDto);

                if (usuariosBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UserListDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários listados com sucesso!";
            }

            return response;
        }

        private static async Task<IEnumerable<UserModel>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<UserModel>("select * from Usuarios");
        }

        public Task<ResponseModel<List<UserListDto>>> ListarUsuarios()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<List<UserListDto>>> EditarUsuario(UserEditDto userEditDto)
        {
            ResponseModel<List<UserListDto>> response = new ResponseModel<List<UserListDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                var usuariosBD = await connection.ExecuteAsync("UPDATE Usuarios SET NomeCompleto = " +
                    "@NomeCompleto, Email = @Email, Cargo = @Cargo, Salario = @Salario, " +
                    "Situacao = @Situacao, CPF = @CPF WHERE id = @Id", userEditDto);

                if (usuariosBD == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar a edição";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UserListDto>>(usuarios);

                response.Dados = usuariosMapeados;

                response.Mensagem = "Usuarios listados com sucesso";

            }

            return response;
        }

        public async Task<ResponseModel<List<UserListDto>>> ExcluirUsuario(int id)
        {
            ResponseModel<List<UserListDto>> response = new ResponseModel<List<UserListDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioDB = await connection.ExecuteAsync("DELETE FROM Usuarios WHERE id = @Id", new { Id = id });

                if (usuarioDB == 0)
                {
                    response.Mensagem = "Usuário não encontrado";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UserListDto>>(usuarios);

                response.Dados = usuariosMapeados;

                response.Mensagem = "Usuário excluido com sucesso";
            }
            return response;
        }
    }
}