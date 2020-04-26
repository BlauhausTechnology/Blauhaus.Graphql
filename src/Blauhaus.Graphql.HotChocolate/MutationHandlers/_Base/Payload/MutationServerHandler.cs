using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base;
using CSharpFunctionalExtensions;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload
{
    public class MutationServerHandler : BaseMutationServerHandler
    { 
        public MutationServerHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }

        protected override Task<Result<TPayload>> HandleCommandAsync<TPayload, TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            var commandHandler = context.Service<ICommandHandler<TPayload, TCommand>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            return commandHandler.HandleAsync(command, token);
        }

    }
}