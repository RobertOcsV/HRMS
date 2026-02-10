namespace HRMS.Application.Common.Interfaces;

using HRMS.Domain.Enums;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Email { get; }
    Role? Role { get; }
    bool IsAuthenticated { get; }
}
