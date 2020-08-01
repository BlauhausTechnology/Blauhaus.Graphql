using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Domain.Common.CommandHandlers;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload._Base
{
    public abstract class BaseAuthenticatedServerQueryHandler<TUser> : BaseServerQueryHandler
    {
 
        
        protected BaseAuthenticatedServerQueryHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }

        protected override Task<Result<TPayload>> HandleCommandAsync<TPayload, TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            if (!TryExtractUser(context, out var authenticatedUser))
            {
                context.ReportError(new ErrorBuilder().SetMessage(AuthErrors.NotAuthenticated.ToString()).Build());
                return Task.FromResult(AnalyticsService.TraceErrorResult<TPayload>(this, AuthErrors.NotAuthenticated));
            };

            var commandHandler = context.Service<IAuthenticatedCommandHandler<TPayload, TCommand, TUser>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            return commandHandler.HandleAsync(command, authenticatedUser, token);
        }

        protected abstract bool TryExtractUser(IResolverContext resolverContext, out TUser user);

    }
}