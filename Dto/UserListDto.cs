
namespace DapperCrudApi.Dto
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Salario { get; set; }
        public bool Situacao { get; set; }

        public static implicit operator UserListDto(List<UserListDto> v)
        {
            throw new NotImplementedException();
        }
    }
}
