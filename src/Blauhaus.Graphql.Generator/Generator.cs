using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQlClientGenerator;

namespace Blauhaus.Graphql.Generator
{
    public static class Generator
    {
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

                var schema = await GraphqlQueryFetcher.RetrieveSchema(config.SchemaUrl, httpClient);

                var builder = new StringBuilder();

                GraphQlGenerator.GenerateQueryBuilder(schema, builder);

                builder.AppendLine();
                builder.AppendLine();

                GraphQlGenerator.GenerateDataClasses(schema, builder);

                using var writer = File.CreateText(config.DestinationPath);
                writer.WriteLine(GraphQlGenerator.RequiredNamespaces);
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