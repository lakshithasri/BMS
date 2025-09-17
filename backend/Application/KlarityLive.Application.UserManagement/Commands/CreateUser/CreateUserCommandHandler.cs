using KlarityLive.Domain.Core.Entities.Amin;
using KlarityLive.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.Extensions.Logging;


namespace KlarityLive.Application.UserManagement.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly KlarityLiveDbContext _context;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(KlarityLiveDbContext context, ILogger<CreateUserCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new User
                {
                    Email = request.User.Email,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User created with ID: {Id}", user.Id);
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }
    }
}
