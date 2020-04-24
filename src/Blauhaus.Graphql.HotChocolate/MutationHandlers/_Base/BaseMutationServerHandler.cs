using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Domain.CommandHandlers;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base
{
    public abstract class BaseMutationServerHandler<TUser> : IMutationServerHandler
    {
        protected readonly IAnalyticsService AnalyticsService;

        protected BaseMutationServerHandler(
            IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
        }

        public async Task<TPayload> HandleAsync<TPayload, TCommand>(IResolverContext context, CancellationToken token)
        {
            try
            {
                var httpContext = (HttpContext)context.ContextData[nameof(HttpContext)];

                var headers = new Dictionary<string, string>();
                foreach (var header in  httpContext.Request.Headers)
                {
                    headers.Add(header.Key, header.Value.FirstOrDefault());
                }

                using (var _ = AnalyticsService.StartRequestOperation(this, typeof(TCommand).Name, headers))
                {

                    if (!TryExtractUser(context, out var authenticatedUser))
                    {
                        throw new UnauthorizedAccessException();
                    };

                    var command = context.Argument<TCommand>("command");
                    if (command == null)
                    {
                        throw new ArgumentException("Unable to extract command from resolver context");
                    }

                    var commandHandler = context.Service<IAuthenticatedCommandHandler<TPayload, TCommand, TUser>>();
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
            catch (Exception e)
            {
                AnalyticsService.LogException(this, e);
                throw;
            }
        }

        protected abstract bool TryExtractUser(IResolverContext resolverContext, out TUser user);
    }
}