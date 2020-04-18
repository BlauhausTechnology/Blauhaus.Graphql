using System.Collections.Generic;
using Blauhaus.Auth.Abstractions.Builders;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.TestHelpers.MockBuilders;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.Tests.MockBuilders
{
    public class ResolverContextMockBuilder : BaseMockBuilder<ResolverContextMockBuilder, IResolverContext>
    {
        private Dictionary<string, object> _contextData;
        public ResolverContextMockBuilder()
        {
            _contextData = new Dictionary<string, object>
            {
                { "HttpContext", new HttpContextMockBuilder().Object },
                { "ClaimsPrincipal", new ClaimsPrincipalBuilder().Build() }
            };
            With_ContextData(_contextData);
        }

        public ResolverContextMockBuilder With_Argument<TArgument>(TArgument argument, string argumentName)
        {
            Mock.Setup(x => x.Argument<TArgument>(argumentName)).Returns(argument);
            return this;
        }

        public ResolverContextMockBuilder With_Command_Argument<TArgument>(TArgument argument)
        {
            Mock.Setup(x => x.Argument<TArgument>("command")).Returns(argument);
            return this;
        }

        public ResolverContextMockBuilder With_Service<TService>(TService service)
        {
            Mock.Setup(x => x.Service<TService>()).Returns(service);
            return this;
        }

        public ResolverContextMockBuilder With_ContextData(string name, object value)
        {
            _contextData[name] = value;
            With_ContextData(_contextData);
            return this;
        }

        public ResolverContextMockBuilder With_ContextData(Dictionary<string, object> value)
        {
            Mock.Setup(x => x.ContextData).Returns(value);
            return this;
        }
    }
}