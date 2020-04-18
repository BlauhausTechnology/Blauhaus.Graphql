using System.Security.Claims;
using Blauhaus.Auth.Abstractions.Builders;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.Tests.MockBuilders
{
    public class HttpContextMockBuilder : BaseMockBuilder<HttpContextMockBuilder, HttpContext>
    {
        public HttpContextMockBuilder()
        {
            WithHeaders(new HeaderDictionary());
            WithClaimsPrincipal(new ClaimsPrincipalBuilder().Build());
        }

        public HttpContextMockBuilder WithHeaders(HeaderDictionary headers)
        {
            
            With(x => x.Request, new MockBuilder<HttpRequest>()
                .With(x => x.Headers, headers).Object);

            return this;
        }

        
        public HttpContextMockBuilder WithClaimsPrincipal(ClaimsPrincipal value)
        {
            
            With(x => x.User, value);

            return this;
        }
    }
}