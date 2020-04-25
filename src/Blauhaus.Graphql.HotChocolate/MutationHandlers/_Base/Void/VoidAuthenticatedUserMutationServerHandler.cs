using System.Security.Claims;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void
{
    public class VoidAuthenticatedUserMutationServerHandler : BaseVoidAuthenticatedMutationServerHandler<IAuthenticatedUser>
    {
        private readonly IAzureAuthenticationServerService _authenticationServerService;

        public VoidAuthenticatedUserMutationServerHandler(
            IAnalyticsService analyticsService,
            IAzureAuthenticationServerService authenticationServerService) 
                : base(analyticsService)
        {
            _authenticationServerService = authenticationServerService;
        }

 
        protected override bool TryExtractUser(IResolverContext resolverContext, out IAuthenticatedUser user)
        {
            if (!resolverContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                claimsPrincipal == null ||
                !((ClaimsPrincipal) claimsPrincipal).Identity.IsAuthenticated)
            {
                user = null;
                return false;
            }

            user = _authenticationServerService.ExtractUserFromClaimsPrincipal((ClaimsPrincipal) claimsPrincipal);
            return true;
        }

    }
}