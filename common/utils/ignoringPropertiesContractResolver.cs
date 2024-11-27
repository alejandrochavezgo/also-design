namespace common.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

public class ignoringPropertiesContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _propertiesToIgnore;

    public ignoringPropertiesContractResolver(IEnumerable<string> propertiesToIgnore)
    {
        _propertiesToIgnore = new HashSet<string>(propertiesToIgnore);
    }

    protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (_propertiesToIgnore.Contains(property.PropertyName!))
        {
            property.ShouldSerialize = _ => false;
        }

        if (property.DeclaringType != null)
        {
            var fullPath = $"{property.DeclaringType.Name}.{property.PropertyName}";

            if (_propertiesToIgnore.Contains(fullPath))
            {
                property.ShouldSerialize = instance => false;
            }
        }

        return property;
    }
}