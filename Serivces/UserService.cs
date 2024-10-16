using AutoMapper;
using Dapper;
using DapperCrudApi.Dto;
using DapperCrudApi.Models;
using System.Data.SqlClient;

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
        public async Task<ResponseModel<UserListDto>> BuscarUsuarioPorId(int Id)
        {
            ResponseModel<UserListDto> response = new ResponseModel<UserListDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBD = await connection.QueryFirstOrDefaultAsync<UserModel>("select * from Usuarios where Usuarios.Id= @Id", new { Id });

                if (usuarioBD == null)
                {
                    response.Mensagem = "Nenhum usuário localizado";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UserListDto>(usuarioBD);

                response.Dados = usuarioMapeado;
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
    }
}