using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Auth.Abstractions.Services;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers
{
    public class AuthenticatedUserMutationServerHandler : IMutationServerHandler
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IAzureAuthenticationServerService _azureAuthenticationService;

        public AuthenticatedUserMutationServerHandler(
            IAnalyticsService analyticsService, 
            IAzureAuthenticationServerService azureAuthenticationService)
        {
            _analyticsService = analyticsService;
            _azureAuthenticationService = azureAuthenticationService;
        }

        public async Task<TPayload> HandleAsync<TCommand, TPayload>(IResolverContext context, CancellationToken token)
        {
            try
            {
                var httpContext = (HttpContext)context.ContextData[nameof(HttpContext)];

                var headers = new Dictionary<string, string>();
                foreach (var header in  httpContext.Request.Headers)
                {
                    headers.Add(header.Key, header.Value.FirstOrDefault());
                }

                using (var _ = _analyticsService.StartRequestOperation(this, typeof(TCommand).Name, headers))
                {
                    if (!context.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                        !((ClaimsPrincipal)claimsPrincipal).Identity.IsAuthenticated)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else
                    {
                        var authenticatedUser = _azureAuthenticationService.ExtractUserFromClaimsPrincipal((ClaimsPrincipal) claimsPrincipal);

                        var command = context.Argument<TCommand>("command");
                        if (command == null)
                        {
                            throw new ArgumentException("Unable to extract command from resolver context");
                        }

                        var commandHandler = context.Service<IAuthenticatedUserCommandHandler<TPayload, TCommand>>();
                        if (commandHandler == null)
                        {
                            throw new ArgumentException("No command handler found for command");
                        }

                        var commandResult = await commandHandler.HandleAsync(command, authenticatedUser, token);
                        if (commandResult.IsFailure)
                        {
                            
                            context.ReportError(new ErrorBuilder().SetMessage(commandResult.Error).Build());
                            return default;
                        }
                        
                        return commandResult.Value;

                    }

                }
            }
            catch (Exception e)
            {
                _analyticsService.LogException(this, e);
                throw;
            }
        }
    }
}