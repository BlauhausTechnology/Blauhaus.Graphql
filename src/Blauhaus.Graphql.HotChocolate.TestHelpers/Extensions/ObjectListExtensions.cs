using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Execution;
using NUnit.Framework;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Extensions
{
    public static class ObjectListExtensions
    {
        public static OrderedDictionary FindById(this List<object> orderedDictionaries, Guid propertyValue)
        {
            var propertyName = "Id";
            return orderedDictionaries.FindByKey(propertyName, propertyValue, x => Guid.Parse(x.ToString()));
        }

        public static OrderedDictionary FindByGuidKey<T>(this List<object> orderedDictionaries,  Expression<Func<T, object>> expression, Guid propertyValue, Func<object, T> converter)
        {
            var propertyName = expression.ToPropertyName();
            return orderedDictionaries.FindByKey(propertyName, propertyValue, x => Guid.Parse(x.ToString()));
        }

        
        public static OrderedDictionary FindByKey<T>(this List<object> orderedDictionaryObjects,  string propertyName, T propertyValue,  Func<object, T> converter)
        {
            OrderedDictionary foundDictionary = null;

            foreach (var dictionaryObject in orderedDictionaryObjects)
            {
                var dictionary = (OrderedDictionary) dictionaryObject;
                if(foundDictionary != null)
                    break;
                
                foreach (var _ in dictionary.Keys)
                {
                    if (dictionary.TryGetValue(propertyName, out var dictionaryValue))
                    {
                        if (converter.Invoke(dictionaryValue).Equals(propertyValue))
                        {
                            foundDictionary = dictionary;
                            break;
                        }
                    }
                }
            }

            if(foundDictionary ==  null)
                Assert.Fail($"The key {propertyName} was not found");

            return foundDictionary;
        }


    }
}