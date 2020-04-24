using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base;
using CSharpFunctionalExtensions;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base
{
    public abstract class BaseAuthenticatedMutationServerHandler<TUser> : BaseMutationServerHandler
    {
 
        
        protected BaseAuthenticatedMutationServerHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }

        protected override Task<Result<TPayload>> HandleCommandAsync<TPayload, TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            if (!TryExtractUser(context, out var authenticatedUser))
            {
                throw new UnauthorizedAccessException();
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