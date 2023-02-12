using System.Reflection;
using System.Runtime.CompilerServices;
using AutoBogus.NSubstitute;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class PrivatePropertyBinder : NSubstituteBinder
{
    public override Dictionary<string, MemberInfo> GetMembers(Type t)
    {
        var members = base.GetMembers(t);

        var privateBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        var allPrivateMembers = t.GetMembers(privateBindingFlags)
            .OfType<FieldInfo>()
            .Where(fi => fi.IsPrivate)
            .Where(fi => !fi.GetCustomAttributes(typeof(CompilerGeneratedAttribute)).Any())
            .ToArray();

        foreach (var privateField in allPrivateMembers)
        {
            members.Add(privateField.Name, privateField);
        }
        return members;
    }
}

