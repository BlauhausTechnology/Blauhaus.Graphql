using System.Security.Claims;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload._Base;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload
{
    public class AuthenticatedUserServerQueryHandler : BaseAuthenticatedServerQueryHandler<IAuthenticatedUser>
    {
        private readonly IAuthenticatedUserFactory _userFactory;

        public AuthenticatedUserServerQueryHandler(
            IAnalyticsService analyticsService,
            IAuthenticatedUserFactory userFactory) 
                : base(analyticsService)
        {
            _userFactory = userFactory;
        }

 
        protected override bool TryExtractUser(IResolverContext resolverContext, out IAuthenticatedUser user)
        {
            
            user = null!;

            if (!resolverContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                claimsPrincipal == null || !((ClaimsPrincipal) claimsPrincipal).Identity.IsAuthenticated)
            {
                return false;
            }

            var extractUser = _userFactory.Create((ClaimsPrincipal) claimsPrincipal);
            if (extractUser.IsFailure)
            {
                return false;
            }
            
            user = extractUser.Value;
            return true;

        }

    }
}