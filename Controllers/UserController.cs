using DapperCrudApi.Dto;
using DapperCrudApi.Serivces;
using Microsoft.AspNetCore.Mvc;

namespace DapperCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userService;
        public UserController(IUserInterface userInterface)
        {
            _userService = userInterface;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios()
        {
            var usuarios = await _userService.BuscarUsuarios();

            if (usuarios.Status == false)
            {

                return NotFound(usuarios);
            }

            return Ok(usuarios);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> BuscarUsuariosPorId(int Id)
        {
            var usuario = await _userService.BuscarUsuarioPorId(Id);

            if (usuario.Status == false)
            {
                return NotFound(usuario);
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UserCreationDto userCreationDto)
        {
            var usuario = await _userService.CriarUsuario(userCreationDto);

            if (usuario.Status == false)
            {
                return BadRequest(usuario);
            }

            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UserEditDto userEditDto)
        {
            var usuarios = await _userService.EditarUsuario(userEditDto);

            if (usuarios.Status == false)
            {
                return BadRequest(usuarios);
            }
            return Ok(usuarios);
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuarios = await _userService.ExcluirUsuario(id);

            if (usuarios.Status == false)
            {
                return BadRequest(usuarios);
            }

            return Ok(usuarios);
        }
    }
}
