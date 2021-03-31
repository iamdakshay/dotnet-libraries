using System.Text.Json;
using System.Collections.Generic;

namespace Aksd.Extensions.Configuration.AmazonSecretsManager
{
    internal sealed class JsonConfigurationParser
    {
        private readonly IDictionary<string, string> _data = new Dictionary<string, string>();

        public IDictionary<string, string> Parse(string jsonString)
        {
            var jsonDocOptions = new JsonDocumentOptions()
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            var jsonDoc = JsonDocument.Parse(jsonString);
            ProcessElement(jsonDoc.RootElement);
            return _data;
        }

        private void ProcessElement(JsonElement element, string parentPath = "")
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var childElement in element.EnumerateObject())
                    {
                        ProcessElement(childElement.Value, (string.IsNullOrEmpty(parentPath) ? parentPath : $"{parentPath}:") + childElement.Name);
                    }
                    break;

                case JsonValueKind.Array:
                    var childElementIndex = 0;
                    foreach (var childElement in element.EnumerateArray())
                    {
                        ProcessElement(childElement, (string.IsNullOrEmpty(parentPath) ? parentPath : $"{parentPath}:") + childElementIndex++);
                    }
                    break;

                case JsonValueKind.String:
                    var elementStr = element.ToString();
                    if (elementStr.StartsWith("{") || elementStr.StartsWith("["))
                    {
                        var jsonDoc = JsonDocument.Parse(elementStr);
                        ProcessElement(jsonDoc.RootElement, parentPath);
                        return;
                    }
                    _data.Add(parentPath, elementStr);
                    break;

                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    _data.Add(parentPath, element.ToString());
                    break;
            }
        }
    }
}
