using KlarityLive.Domain.Core.Entities.Amin;
using MediatR;

namespace KlarityLive.Application.UserManagement.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public User User { get; set; }
    }
}
