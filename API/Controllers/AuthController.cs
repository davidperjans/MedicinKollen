using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using AutoMapper;
using Application.Queries.UserQueries.AuthenticateUser;
using Application.Commands.UserCommands.RegisterUser;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var command = _mapper.Map<RegisterUserCommand>(dto);

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var query = _mapper.Map<AuthenticateUserQuery>(dto);

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(new { Token = result.Data!.Token, User = result.Data.User });

            return BadRequest(result.ErrorMessage);
        }
    }
}
