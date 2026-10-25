using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Queries.GetCurrentUserQuery
{
    public class GetCurrentUserDto
    {
        public string UserId { get; set; }=string.Empty;
        public string FullName { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        

    }
}
