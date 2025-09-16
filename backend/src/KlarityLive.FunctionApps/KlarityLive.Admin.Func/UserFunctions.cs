using KlarityLive.Application.Features.Users.Commands.CreateUser;
using KlarityLive.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Admin.Func;

public class UserFunctions
{
    private readonly ILogger<UserFunctions> _logger;
    protected readonly IMediator _mediator;

    public UserFunctions(IMediator mediator, ILogger<UserFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function("CreateUserAsync")]
    public async Task<IActionResult> CreateUserAsync([HttpTrigger(AuthorizationLevel.Function, "post", "users")] HttpRequest req)
    {
        _logger.LogInformation("Executing CreateUserAsync");
        var user = new User
        {
            CreatedDate = DateTime.UtcNow,
            Email = "srinath@pearltechglobal.com",
            FirstName = "Srinath",
            LastName = "Dharme",
            Mobile = "+94711332493"
        };

        var result = await _mediator.Send(new CreateUserCommand { User = user });

        return new OkObjectResult(result);
    }
}