using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Execution;
using NUnit.Framework;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Extensions
{
    public static class OrderedDictionaryExtensions
    {
        
        public static bool TryGetProperty<T>(this OrderedDictionary orderedDictionary, Expression<Func<T, object>> expression, out object property)
        {
            return orderedDictionary.TryGetProperty(expression.ToPropertyName(), out property);
        }

        public static object GetProperty<T>(this OrderedDictionary orderedDictionary, Expression<Func<T, object>> expression)
        {
            return orderedDictionary.GetProperty(expression.ToPropertyName());
        }

        
        public static bool TryGetProperty(this OrderedDictionary orderedDictionary, string propertyName, out object property)
        {
            var index = -1;
            var foundKey=false;
            
            foreach (var key in orderedDictionary.Keys)
            {
                index++;
                if (key == propertyName)
                {
                    foundKey = true;
                    break;
                }
            }

            if (!foundKey)
            {
                property = null;
                return false;
            }

            property = orderedDictionary.Values.ToArray()[index];
            return true;
        }

        public static object GetProperty(this OrderedDictionary orderedDictionary, string propertyName)
        {
            var index = -1;
            var foundKey=false;
            
            foreach (var key in orderedDictionary.Keys)
            {
                index++;
                if (key == propertyName)
                {
                    foundKey = true;
                    break;
                }
            }

            if(!foundKey)
                Assert.Fail($"The key {propertyName} was not found");

            return  orderedDictionary.Values.ToArray()[index];
        }
        
        public static void VerifyProperty<T, TProperty>(this OrderedDictionary orderedDictionary, Expression<Func<T, TProperty>>  expression, TProperty propertyValue)
        {
            var propertyName = expression.ToPropertyName();
            var property = orderedDictionary.GetProperty(propertyName);

            if(typeof(TProperty) == typeof(Guid?))
            {
                Assert.That( Guid.Parse(property.ToString()), Is.EqualTo(propertyValue));
            }
            else
            {
                Assert.That(property, Is.EqualTo(propertyValue));
            }
        }

        public static void VerifyPropertyDateTime(
            this OrderedDictionary orderedDictionary,
            string propertyName,
            DateTime? propertyValue)
        {
            Assert.That((string) orderedDictionary.GetProperty(propertyName), Is.EqualTo(propertyValue?.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffZ", CultureInfo.InvariantCulture)));
        }

        public static void VerifyId(this OrderedDictionary graphqlObject,  Guid id)
        {
            var idObjectKvp = graphqlObject
                .FirstOrDefault(x => x.Key.ToLowerInvariant() == "id");

            if(string.IsNullOrEmpty(idObjectKvp.Key))
                Assert.Fail("Id was not found on this object");

            var idObject = Guid.Parse(idObjectKvp.Value.ToString());

            Assert.That(idObject, Is.EqualTo(id));
        }

        public static void VerifyProperty(this OrderedDictionary orderedDictionary,  string propertyName, object propertyValue)
        {
            var property = orderedDictionary.GetProperty(propertyName);
            Assert.That(property, Is.EqualTo(propertyValue));
        }
        
        public static void VerifyPropertyDateTimeOffset(this OrderedDictionary orderedDictionary,  string propertyName, DateTimeOffset propertyValue)
        {
            var objectProperty = (string)orderedDictionary.GetProperty(propertyName);
            Assert.That(objectProperty, Is.EqualTo(propertyValue.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffZ", CultureInfo.InvariantCulture)));
        }

        public static void VerifyPropertyDateTime(this OrderedDictionary orderedDictionary,  string propertyName, DateTime propertyValue)
        {
            var objectProperty = (string)orderedDictionary.GetProperty(propertyName);
            Assert.That(objectProperty, Is.EqualTo(propertyValue.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffZ", CultureInfo.InvariantCulture)));
        }

        public static void VerifyPropertyGuid(this OrderedDictionary orderedDictionary,  string propertyName, Guid propertyValue)
        {
            var objectProperty = (string)orderedDictionary.GetProperty(propertyName);
            Assert.That(Guid.Parse(objectProperty), Is.EqualTo(propertyValue));
        }

        
        public static void VerifyEntityId(this OrderedDictionary graphqlObject, Guid id)
        {
            var keyValuePair = graphqlObject.FirstOrDefault(x => x.Key.ToLowerInvariant() == "entityid");
            if (string.IsNullOrEmpty(keyValuePair.Key))
                Assert.Fail("EntityId was not found on this object");
            Assert.That<Guid>(Guid.Parse(keyValuePair.Value.ToString()), Is.EqualTo(id));
        }


    }
}