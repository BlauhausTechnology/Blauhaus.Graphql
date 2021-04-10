using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base;
using Blauhaus.Responses;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void
{
    public class VoidAnonymousServerQueryHandler : BaseVoidServerQueryHandler
    { 
        public VoidAnonymousServerQueryHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }


        protected override async Task<Response<bool>> HandleCommandAsync<TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            var commandHandler = context.Service<IVoidCommandHandler<TCommand>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            var result = await commandHandler.HandleAsync(command);
            return result.IsSuccess
                ? Response.Success(true)
                : Response.Failure<bool>(result.Error);
        }
    }
}