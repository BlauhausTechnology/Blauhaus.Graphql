using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.MutationHandlers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestMutationClientHandler : BaseMutationClientHandler<TestDto, TestResponse, TestCommandInput>
    {
        private IOperationResult<TestResponse> _result = new MockBuilder<IOperationResult<TestResponse>>().Object;

        public TestMutationClientHandler Where_GetResultAsync_returns(IOperationResult<TestResponse> result)
        {
            _result = result;
            return this;
        }


        protected override Task<IOperationResult<TestResponse>> GetResultAsync(TestCommandInput operation, CancellationToken token)
        {
            return Task.FromResult(_result);
        }

        protected override Result<TestDto> ExtractDto(IOperationResult<TestResponse> operationResult)
        {
            return Result.Success(_result.Data.Dto);
        }
    }
}