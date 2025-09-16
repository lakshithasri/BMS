using KlarityLive.Domain.Entities;
using MediatR;

namespace KlarityLive.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public User User { get; set; }
    }
}
