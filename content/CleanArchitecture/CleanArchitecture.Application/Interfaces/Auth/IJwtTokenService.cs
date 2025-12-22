using CleanArchitecture.Domain.Entities.Auth;
using CleanArchitecture.Persistence.Identity;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user);
    RefreshToken GenerateRefreshToken(Guid userId);
}