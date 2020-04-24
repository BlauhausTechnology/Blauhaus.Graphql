using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Execution;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Extensions
{

    public class Edge
    {
        public string Cursor { get; set; }
        public OrderedDictionary Node { get; set; }
    }
    
    public class PageInfo
    {
        public string EndCursor { get; set; }
        public bool? HasNextPage { get; set; }
        public bool? HasPreviousPage { get; set; }
        public string StartCursor { get; set; }
    }

    public class ConnectionResult
    {
        public ICollection<Edge> Edges { get; set; } = new List<Edge>();
        public ICollection<OrderedDictionary> Nodes { get; set; } = new List<OrderedDictionary>();
        public PageInfo PageInfo { get; set; }
        public int? TotalCount { get; set; }
    }


    public static class ExecutionResultPaginationExtensions
    {
        

        public static List<TProperty> GetNodeProperties<TQuery, T, TProperty>(this IExecutionResult executionResult, 
            Expression<Func<TQuery, object>> queryExpression, Expression<Func<T, TProperty>> propertyExpression)
        {
            var connectionResult = executionResult.GetConnectionResult (queryExpression);
            
            var nodeProperties = new List<TProperty>();
            foreach (var connectionResultNode in connectionResult.Edges)
            {
                nodeProperties.Add((TProperty)connectionResultNode.Node.GetProperty(propertyExpression.ToPropertyName()));
            }
            return nodeProperties;
        }


        public static List<DateTime?> GetNodeDateTimeProperties<TQuery, T>(this IExecutionResult executionResult, 
            Expression<Func<TQuery, object>> queryExpression, Expression<Func<T, DateTime?>> propertyExpression)
        {
            var connectionResult = executionResult.GetConnectionResult (queryExpression);
            
            var nodeProperties = new List<DateTime?>();
            foreach (var connectionResultNode in connectionResult.Edges)
            {
                var dateTimeString = connectionResultNode.Node.GetProperty(propertyExpression.ToPropertyName())?.ToString();
                if (dateTimeString == null)
                {
                    nodeProperties.Add(null);
                }
                else
                {
                    var dateTime = DateTime.Parse(dateTimeString);
                    var utcDateTime = dateTime.ToUniversalTime();
                    nodeProperties.Add(utcDateTime);
                }
              
            }
            return nodeProperties;
        }

        
        public static ConnectionResult GetConnectionResult<TQuery>(this OrderedDictionary orderedDictionary, Expression<Func<TQuery, object>> expression)
        {
            var paginatedResult = (OrderedDictionary)orderedDictionary.GetProperty(expression);

            var connectionResult = new ConnectionResult();

            if(paginatedResult.TryGetProperty("nodes", out var nodesResult))
            {
                var nodes = (List<object>)nodesResult;
                foreach (var node in nodes)
                {
                    connectionResult.Nodes.Add((OrderedDictionary)node);
                }
            }

            var pageInfo = (OrderedDictionary)paginatedResult.GetProperty("pageInfo");
            connectionResult.PageInfo = new PageInfo
            {
                HasNextPage = (bool)pageInfo.GetProperty("hasNextPage"),
                HasPreviousPage = (bool)pageInfo.GetProperty("hasPreviousPage"),
                StartCursor = (string)pageInfo.GetProperty("startCursor"),
                EndCursor = (string)pageInfo.GetProperty("endCursor")
            };

            var totalCount = (int)paginatedResult.GetProperty("totalCount");
            connectionResult.TotalCount = totalCount;

            var edges = (List<object>)paginatedResult.GetProperty("edges");
            foreach (var edge in edges)
            {
                connectionResult.Edges.Add(new Edge
                {
                    Cursor = (string) ((OrderedDictionary)edge).GetProperty("cursor"),
                    Node = (OrderedDictionary)((OrderedDictionary)(edge)).GetProperty("node"),
                });
            }
            return connectionResult;
        }

        public static ConnectionResult GetConnectionResult<TQuery>(this IExecutionResult executionResult, Expression<Func<TQuery, object>> expression)
        {
            var paginatedResult = executionResult.GetPropertyDictionary(expression);

            var connectionResult = new ConnectionResult();

            if(paginatedResult.TryGetProperty("nodes", out var nodesResult))
            {
                var nodes = (List<object>)nodesResult;
                foreach (var node in nodes)
                {
                    connectionResult.Nodes.Add((OrderedDictionary)node);
                }
            }

            var pageInfo = (OrderedDictionary)paginatedResult.GetProperty("pageInfo");
            connectionResult.PageInfo = new PageInfo
            {
                HasNextPage = (bool)pageInfo.GetProperty("hasNextPage"),
                HasPreviousPage = (bool)pageInfo.GetProperty("hasPreviousPage"),
                StartCursor = (string)pageInfo.GetProperty("startCursor"),
                EndCursor = (string)pageInfo.GetProperty("endCursor")
            };
            
            var totalCount = (int)paginatedResult.GetProperty("totalCount");
            connectionResult.TotalCount = totalCount;
            
            var edges = (List<object>)paginatedResult.GetProperty("edges");
            foreach (var edge in edges)
            {
                connectionResult.Edges.Add(new Edge
                {
                    Cursor = (string) ((OrderedDictionary)edge).GetProperty("cursor"),
                    Node = (OrderedDictionary)((OrderedDictionary)(edge)).GetProperty("node"),
                });
            }
            return connectionResult;
        }


    }
}
