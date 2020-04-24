using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base;
using CSharpFunctionalExtensions;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void
{
    public class VoidMutationServerHandler : BaseVoidMutationServerHandler
    { 
        public VoidMutationServerHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }


        protected override async Task<Result<bool>> HandleCommandAsync<TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            var commandHandler = context.Service<IVoidCommandHandler<TCommand>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            var result = await commandHandler.HandleAsync(command, token);
            return result.IsSuccess
                ? Result.Success(true)
                : Result.Failure<bool>(result.Error);
        }
    }
}