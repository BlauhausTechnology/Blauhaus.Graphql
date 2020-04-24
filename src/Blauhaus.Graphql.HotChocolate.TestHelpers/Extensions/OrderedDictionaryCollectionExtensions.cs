using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Execution;
using NUnit.Framework;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Extensions
{
    public static class OrderedDictionaryCollectionExtensions
    {
        
        public static OrderedDictionary FindByEntityId(this IEnumerable<OrderedDictionary> orderedDictionaries, Guid entityId)
        {
            foreach (var orderedDictionary in orderedDictionaries)
            {
                var keyValuePair = orderedDictionary.FirstOrDefault(x => x.Key.ToLowerInvariant() == "entityid");
                if (!string.IsNullOrEmpty(keyValuePair.Key) && Guid.Parse(keyValuePair.Value.ToString()) == entityId)
                    return orderedDictionary;
            }
            Assert.Fail("EntityId was not found on this object");
            return null;
        }
        
        public static OrderedDictionary FindById(this IEnumerable<OrderedDictionary> orderedDictionaries, Guid id)
        {

            foreach (var orderedDictionary in orderedDictionaries)
            {
                var idObjectKvp = orderedDictionary
                    .FirstOrDefault(x => x.Key.ToLowerInvariant() == "id");

                if (!string.IsNullOrEmpty(idObjectKvp.Key))
                {
                    var idObject = Guid.Parse(idObjectKvp.Value.ToString());
                    if (idObject == id)
                        return orderedDictionary;
                }
            }
            Assert.Fail("Id was not found on this object");
            return null;
        }
      
        public static OrderedDictionary FindByGuidKey<T>(this List<OrderedDictionary> orderedDictionaries, Expression<Func<T, Guid>> expression, Guid propertyValue)
        {
            var propertyName = expression.ToPropertyName();
            return orderedDictionaries.FindByKey(propertyName, propertyValue, x => Guid.Parse(x.ToString()));
        }

        public static OrderedDictionary FindByKey<T>(this List<OrderedDictionary> orderedDictionaries, string propertyName, T propertyValue, Func<object, T> converter)
        {
            var dictionaryindex = -1;
            var foundKey = false;

            foreach (var dictionary in orderedDictionaries)
            {

                if (foundKey)
                    break;

                dictionaryindex++;
                foreach (var _ in dictionary.Keys)
                {
                    if (dictionary.TryGetValue(propertyName, out var dictionaryValue))
                    {
                        if (converter.Invoke(dictionaryValue).Equals(propertyValue))
                        {
                            foundKey = true;
                            break;
                        }
                        
                    }
                }
            }

            if (!foundKey)
                Assert.Fail($"The key {propertyName} was not found");

            return orderedDictionaries[dictionaryindex];
        }

    }
}