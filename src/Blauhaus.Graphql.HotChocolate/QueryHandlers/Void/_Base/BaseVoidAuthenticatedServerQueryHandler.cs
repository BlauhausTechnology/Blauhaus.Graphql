using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Responses;
using HotChocolate;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base
{
    public abstract class BaseVoidAuthenticatedServerQueryHandler<TUser> : BaseVoidServerQueryHandler
    {
        protected BaseVoidAuthenticatedServerQueryHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }

        protected override async Task<Response<bool>> HandleCommandAsync<TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            if (!TryExtractUser(context, out var authenticatedUser))
            {
                context.ReportError(new ErrorBuilder().SetMessage(AuthErrors.NotAuthenticated.ToString()).Build());
                return AnalyticsService.TraceErrorResponse<bool>(this, AuthErrors.NotAuthenticated);
            };

            var commandHandler = context.Service<IVoidAuthenticatedCommandHandler<TCommand, TUser>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            var result = await commandHandler.HandleAsync(command, authenticatedUser);
            
            return result.IsFailure 
                ? Response.Failure<bool>(result.Error) 
                : Response.Success(true);
        }

        protected abstract bool TryExtractUser(IResolverContext resolverContext, out TUser user);
    }
}