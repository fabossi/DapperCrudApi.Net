namespace DapperCrudApi.Dto
{
    public class UserCreationDto
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string CPF { get; set; }
        public string Salario { get; set; }
        public bool Situacao { get; set; }
        public string Senha { get; set; }
    }
}
