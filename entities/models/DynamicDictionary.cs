namespace entities.models;

using System.Dynamic;

public class dynamicDictionary : DynamicObject
{
    //Input Dictionary.
    Dictionary<string, object> dictionary = new Dictionary<string, object>();

    /// <summary>
    /// Property that returns a dictionary<String,Object> with the actual properties and values of a class instance.
    /// Useful for iterate over the actual properties, for example.
    /// </summary>
    public Dictionary<string, object> getDictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Property that returns a collection of the representation of the class instance members.
    /// </summary>
    public Dictionary<string, object>.KeyCollection miembros
    {
        get
        {
            return this.dictionary.Keys;
        }
    }

    /// <summary>
    /// Property that returns a collection of values' representation of a class instance.
    /// </summary>
    public Dictionary<string, object>.ValueCollection valores
    {
        get
        {
            return this.dictionary.Values;
        }
    }

    /// <summary>
    /// Property that returns the quantity of elements in the input dictionary.
    /// </summary>
    public int count
    {
        get
        {
            return dictionary.Count;
        }
    }

    /// <summary>
    /// If a non defined class property is tried to recover, this methos is called.
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        // Cast the property name to lower case,
        // for case-insensitive comparation.
        string name = binder.Name.ToLower();

        // If the property is not found in the dictionary,
        // this establishes the result of the parameter to the property value and returns true
        // otherwise, it returns false.
        return dictionary.TryGetValue(name, out result);
    }

    /// <summary>
    /// If a value is trying to set to a non defined property
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        // Cast the property name to lower case,
        // for case-insensitive comparation.
        dictionary[binder.Name.ToLower()] = value;

        // A value always can be added to a dictionary, so it always returns true.
        return true;
    }

    /// <summary>
    /// If it is trying to set a value to a non defined property.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool tryAddMember(string Name, dynamic value)
    {
        // Cast the property name to lower case,
        // for case-insensitive comparation.
        dictionary[Name.ToLower()] = value;

        // A value always can be added to a dictionary, so it always returns true.
        return true;
    }
}