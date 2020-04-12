using Blauhaus.TestHelpers.BaseTests;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests._Base
{
    public abstract class BaseGraphqlTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
        }
    }
}