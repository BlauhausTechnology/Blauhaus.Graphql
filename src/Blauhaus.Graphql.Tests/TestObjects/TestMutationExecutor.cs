using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.Executors;
using Blauhaus.Graphql.StrawberryShake.MutationExecutors;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.TestObjects
{
    public class TestMutationExecutor : BaseMutationExecutor<TestOperation, TestPayload>
    {
        private IOperationResult<TestPayload> _result;

        public TestMutationExecutor Where_GetResultAsync_returns(IOperationResult<TestPayload> result)
        {
            _result = result;
            return this;
        }

        protected override Task<IOperationResult<TestPayload>> GetResultAsync(TestOperation operation, CancellationToken token)
        {
            return Task.FromResult(_result);
        }
    }
}