using System.Collections.Generic;
using System.Security.Claims;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Builders
{
    public class QueryBuilder
    {

        private readonly QueryRequestBuilder _queryRequestBuilder;
        private readonly IServiceCollection _serviceCollection;
        private ClaimsPrincipal _claimsPrincipal;

        public QueryBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _queryRequestBuilder = QueryRequestBuilder.New();
        }
        
        public QueryBuilder With_Property(string name, object value)
        {
            _queryRequestBuilder.SetProperty(name, value);
            return this;
        }
        
        public QueryBuilder With_OperationName(string name)
        {
            _queryRequestBuilder.SetOperation(name);
            return this;
        }

        public QueryBuilder With_ClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
            return this;
        }

        public QueryBuilder With_Context_property(string key, object value)
        {
            _queryRequestBuilder.SetProperty(key, value);
            return this;
        }


        public QueryBuilder With_Query(string query)
        {
            _queryRequestBuilder.SetQuery(query);
            return this;
        }

        public QueryBuilder With_Variables(Dictionary<string, object> variables)
        {
            _queryRequestBuilder.SetVariableValues(variables);
            return this;
        }

        public QueryBuilder With_Variable(string name, object value)
        {
            _queryRequestBuilder.SetVariableValue(name, value);
            return this;
        }

        public IReadOnlyQueryRequest Build()
        {
            if (_claimsPrincipal != null)
                _queryRequestBuilder.SetProperty(nameof(ClaimsPrincipal), _claimsPrincipal);

            if (_serviceCollection != null)
                _queryRequestBuilder.SetServices(_serviceCollection.BuildServiceProvider());

            return _queryRequestBuilder.Create();
        }


    }
}