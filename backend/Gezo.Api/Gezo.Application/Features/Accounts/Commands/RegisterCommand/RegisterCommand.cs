using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.RegisterCommand
{
    public record RegisterCommand(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<ApiResponse<RegisterResponseDto>>
    {

    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public RegisterCommandHandler(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail is not null)
                throw new BadRequestCustomException("Email is already registered");

            var user = _mapper.Map<ApplicationUser>(request);

            var  result= await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestCustomException("Unsuccess", errors);
            }
            var setRole = await _userManager.AddToRoleAsync(user, "Customer");

            var token = await _tokenService.GenerateTokenAsync(user);

            var dto = _mapper.Map<RegisterResponseDto>(user);
            dto.Token = token;

            return new ApiResponse<RegisterResponseDto>(
                dto,
                "Account registered successfully",
                StatusCodes.Status201Created);
           

        }
    }
}
