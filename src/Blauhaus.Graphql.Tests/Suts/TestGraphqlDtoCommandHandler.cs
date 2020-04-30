using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestGraphqlDtoCommandHandler : ClientQueryHandler<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>
    {
        public TestGraphqlDtoCommandHandler(
            IGraphqlClient<TestModelDto,  TestGraphqlResponse, TestCommandDto, TestCommand> graphqlClient) 
                : base(graphqlClient)
        {
        }
    }
}