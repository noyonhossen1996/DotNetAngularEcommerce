using ECommerceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user);

        string GenerateRefreshToken();
    }
}
