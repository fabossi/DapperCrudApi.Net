using DapperCrudApi.Dto;
using DapperCrudApi.Models;

namespace DapperCrudApi.Serivces
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserListDto>>> BuscarUsuarios();
        Task<ResponseModel<UserListDto>> BuscarUsuarioPorId(int Id);
        Task<ResponseModel<List<UserListDto>>> CriarUsuario(UserCreationDto userCreationDto);
        Task<ResponseModel<List<UserListDto>>> ListarUsuarios();
        Task<ResponseModel<List<UserListDto>>> EditarUsuario(UserEditDto userEditDto);
        Task<ResponseModel<List<UserListDto>>> ExcluirUsuario(int id);
    }
}
