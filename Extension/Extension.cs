using Amazon.DynamoDBv2.Model;

namespace ReadingDiaryApi.Extension
{
    public static class Extension
    {
        public static T ToClass<T>(this Dictionary<string, AttributeValue> dict)
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var property = type.GetProperty(kv.Key);
                if (property != null)
                {
                    if (!string.IsNullOrEmpty(kv.Value.S))
                    {
                        property.SetValue(obj, kv.Value.S);
                    }
                    else if (!string.IsNullOrEmpty(kv.Value.N))
                    {
                        property.SetValue(obj, int.Parse(kv.Value.N));
                    }
                    else if (kv.Value.SS != null && kv.Value.SS.Count > 0)
                    {
                        var listType = property.PropertyType;
                        var elementType = listType.GetGenericArguments()[0];
                        var list = Activator.CreateInstance(listType);

                        foreach (var item in kv.Value.SS)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var listItem = Convert.ChangeType(item, elementType);
                                listType.GetMethod("Add").Invoke(list, new[] { listItem });
                            }
                        }

                        property.SetValue(obj, list);
                    }
                }
            }
            return (T)obj;
        }
    }
}
