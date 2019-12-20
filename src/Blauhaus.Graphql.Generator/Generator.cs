using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQlClientGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Blauhaus.Graphql.Generator
{
    public static class Generator
    {
        internal const string RequiredNamespaces = "using System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.ComponentModel;\r\nusing System.Globalization;\r\nusing System.Linq;\r\nusing System.Reflection;\r\nusing System.Runtime.Serialization;\r\nusing System.Text;\r\nusing System.Text.RegularExpressions;\r\nusing Newtonsoft.Json;\r\nusing Newtonsoft.Json.Linq;\r\n";


        public static async Task GeneratAsync(this GraphqlGeneratorConfig config)
        {

            try
            {
                GraphQlGeneratorConfiguration.CustomScalarFieldTypeMapping =
                    (baseType, valueType, valueName) =>
                    {
                        valueType = valueType is GraphQlFieldType fieldType ? fieldType.UnwrapIfNonNull() : valueType;

                        return config.FieldTypeMappings.TryGetValue(valueType.Name, out var overrideTypeName) 
                            ? overrideTypeName 
                            : GraphQlGeneratorConfiguration.DefaultScalarFieldTypeMapping(baseType, valueType, valueName);
                    };


                var httpClient = new HttpClient();

                if(!string.IsNullOrEmpty(config.AuthenticationHeander.Key))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(config.AuthenticationHeander.Key, config.AuthenticationHeander.Value);

                foreach (var header in config.HttpHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                using var response =
                    await httpClient.PostAsync(config.SchemaUrl,
                        new StringContent(JsonConvert.SerializeObject(new { query = IntrospectionQuery.Text }), Encoding.UTF8, "application/json"));

                var schema =
                    response.Content == null
                        ? "(no content)"
                        : await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Status code: {(int)response.StatusCode} ({response.StatusCode}); content: {schema}");

                if (config.GenerateSchema)
                {
                    var formattedSchema = JToken.Parse(schema).ToString(Formatting.Indented);
                    using var schemaWriter = File.CreateText(config.DestinationPath +"schema.graphql");
                    await schemaWriter.WriteAsync(formattedSchema);
                }


                var deserializedSchema = GraphQlGenerator.DeserializeGraphQlSchema(schema);

                var builder = new StringBuilder();

                GraphQlGenerator.GenerateQueryBuilder(deserializedSchema, builder);

                builder.AppendLine();
                builder.AppendLine();

                GraphQlGenerator.GenerateDataClasses(deserializedSchema, builder);

                using var writer = File.CreateText(config.DestinationPath + "QueryBuilder.cs");
                writer.WriteLine(RequiredNamespaces);
                writer.WriteLine();

                writer.WriteLine($"namespace {config.Namespace}");
                writer.WriteLine("{");

                var indentedLines =
                    builder
                        .ToString()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .Select(l => $"    {l}");

                foreach (var line in indentedLines)
                    writer.WriteLine(line);

                writer.WriteLine("}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}