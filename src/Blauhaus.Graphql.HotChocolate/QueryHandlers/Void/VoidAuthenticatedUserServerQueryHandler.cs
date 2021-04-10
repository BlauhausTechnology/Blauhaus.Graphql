using System.Security.Claims;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void
{
    public class VoidAuthenticatedUserServerQueryHandler : BaseVoidAuthenticatedServerQueryHandler<IAuthenticatedUser>
    {
        private readonly IAuthenticatedUserFactory _userFactory;

        public VoidAuthenticatedUserServerQueryHandler(
            IAnalyticsService analyticsService,
            IAuthenticatedUserFactory authenticatedUserFactory) 
                : base(analyticsService)
        {
            _userFactory = authenticatedUserFactory;
        }

 
        protected override bool TryExtractUser(IResolverContext resolverContext, out IAuthenticatedUser user)
        {
            
            user = null!;

            if (!resolverContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                claimsPrincipal == null ||
                !((ClaimsPrincipal) claimsPrincipal).Identity.IsAuthenticated)
            {
                return false;
            }
            var extractUser = _userFactory.ExtractFromClaimsPrincipal((ClaimsPrincipal) claimsPrincipal);
            if (extractUser.IsFailure)
            {
                return false;
            }
            
            user = extractUser.Value;
            return true;
        }

    }
}