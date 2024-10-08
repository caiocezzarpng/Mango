﻿using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDTO _responseDTO;
        
        public AuthAPIController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _messageBus = messageBus;
            _configuration = configuration;
            _responseDTO = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage)) 
            {
                _responseDTO.Success = false;
                _responseDTO.Message = errorMessage;
                return BadRequest(_responseDTO);
            }

            await _messageBus.PublishMessage(model, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

            return Ok(_responseDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _responseDTO.Success = false;
                _responseDTO.Message = "Username or password is incorrect";
                return BadRequest(_responseDTO);
            }

            _responseDTO.Result = loginResponse;
            return Ok(_responseDTO);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccesful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccesful)
            {
                _responseDTO.Success = false;
                _responseDTO.Message = "Error encountered";
                return BadRequest(_responseDTO);
            }
            return Ok(_responseDTO);
        }
    }
}
