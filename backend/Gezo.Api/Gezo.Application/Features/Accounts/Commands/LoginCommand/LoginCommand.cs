using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.LoginCommand
{
    public record LoginCommand(string email,string password) : IRequest<ApiResponse<LoginResponseDto>>
    {

    }

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenCookieService _refreshTokenCookieService;
        private readonly IRefreshTokenService _refreshTokenService;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper,IRefreshTokenCookieService refreshTokenCookieService,IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _refreshTokenCookieService = refreshTokenCookieService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<ApiResponse<LoginResponseDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            //step 1 vaildation done using flunevaildation for password and email
            //step 2 check the email if it exit 
            var user = await _userManager.FindByEmailAsync(command.email);
            if (user is null)
                throw new UnauthorizedCustomException("You email or password is not correct");

            var isEmailConfirm = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirm)
                throw new BadRequestCustomException("Please confirm your email ");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedCustomException( "Your email or password is incorrect");

            var isVaildPassword = await _userManager.CheckPasswordAsync(user,command.password);
            if (!isVaildPassword)
            {
                await _userManager.AccessFailedAsync(user);
                throw new UnauthorizedCustomException("You email or password is not correct");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            //step 3 generate token 

            var token = await _tokenService.GenerateTokenAsync(user);
            //step 4 generate  referesh token token 
            var rawRefershToken = await _refreshTokenService.CreateAsync(user, cancellationToken);
            _refreshTokenCookieService.SetRefreshToken(rawRefershToken);

            //step 5 map the ApplicationUser to LoginCommandDto
            var dto = _mapper.Map<LoginResponseDto>(user);
            dto.Token = token;
            
            
            //step 6 return the response
            return new ApiResponse<LoginResponseDto>
                (dto,
                "Login Successful",
                StatusCodes.Status200OK); ;

        }
    }

}
