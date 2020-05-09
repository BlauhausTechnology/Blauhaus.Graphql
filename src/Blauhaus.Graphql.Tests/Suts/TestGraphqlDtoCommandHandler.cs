using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.Tests.TestObjects;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestGraphqlDtoCommandHandler : ClientQueryHandler<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>
    {
        public TestGraphqlDtoCommandHandler(
            IGraphqlQueryConverter<TestModelDto,  TestGraphqlResponse, TestCommandDto, TestCommand> graphqlClient) 
                : base(graphqlClient)
        {
        }
    }
}