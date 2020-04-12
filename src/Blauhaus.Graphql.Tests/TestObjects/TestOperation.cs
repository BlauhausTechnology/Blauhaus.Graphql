using System;
using System.Collections.Generic;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.TestObjects
{
    public class TestOperation : IOperation<TestPayload>
    {
        public IReadOnlyList<VariableValue> GetVariableValues()
        {
            return new List<VariableValue>();
        }

        public string Name { get; }
        public IDocument Document { get; }
        public OperationKind Kind { get; }
        public Type ResultType { get; }
    }
}