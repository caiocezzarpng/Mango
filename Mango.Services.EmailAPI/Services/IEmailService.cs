﻿using Mango.Services.EmailAPI.Models.DTOs;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);

        Task EmailRegistrationAndLog(RegistrationRequestDTO requestDTO);
    }
}
