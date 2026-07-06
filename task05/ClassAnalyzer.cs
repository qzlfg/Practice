using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods().Select(method => method.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname);

        if (method == null)
        {
            return Enumerable.Empty<string>();
        }

        var ParamsOfMethod = method.GetParameters().Select(param => $"{param.ParameterType.Name} value: {param.Name}");

        return ParamsOfMethod.Append($"ReturnType: {method.ReturnType.Name}");
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        .Select(field => field.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties().Select(proeprty => proeprty.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.IsDefined(typeof(T), false);
    }
}