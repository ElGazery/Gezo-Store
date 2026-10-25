using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Gezo.Application.Specifications.RefreshTokens
{
    public class RefreshTokenByHashSpecification:BaseSpecification<RefreshToken>
    {
        public RefreshTokenByHashSpecification(string tokenHash)
        {
            Criteria = token => token.TokenHash == tokenHash;
            AddInclude(e => e.User);
        }

    }
}
