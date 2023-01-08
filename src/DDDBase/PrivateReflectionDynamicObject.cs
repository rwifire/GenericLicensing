using System.Dynamic;
using System.Reflection;

namespace DDDBase;

public class PrivateReflectionDynamicObject : DynamicObject
{
  private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.Public |
                                            System.Reflection.BindingFlags.NonPublic;


  /// <summary>
  /// The original/real <see cref="object"/> this wraps.
  /// </summary>
  public object RealObject { get; set; } = null!;

  internal static object WrapObjectIfNeeded(object @object)
  {
    // Don't wrap primitive types, which don't have many interesting internal APIs
    if (@object.GetType().IsPrimitive || @object is string)
    {
      return @object;
    }

    return new PrivateReflectionDynamicObject {RealObject = @object};
  }

  /// <summary>
  /// Calls <see cref="InvokeMemberOnType"/> then passes the response to <see cref="WrapObjectIfNeeded"/>.
  /// </summary>
  /// <remarks>Called when a method is called.</remarks>
  public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object? result)
  {
    result = InvokeMemberOnType(RealObject.GetType(), RealObject, binder.Name, args);

    if (result != null)
    {
      // Wrap the sub object if necessary. This allows nested anonymous objects to work.
      result = WrapObjectIfNeeded(result);
    }

    return true;
  }

  private static object? InvokeMemberOnType(Type type, object target, string name, object[] args)
  {
    try
    {
      if (type.GetMember(name,
            BindingFlags.Instance |
            BindingFlags.NonPublic | BindingFlags.Public)
          .Any())
      {
        return type.InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags, null,
          target, args);
      }

      // If we couldn't find the method, try on the base class
      if (type.BaseType != null)
      {
        return InvokeMemberOnType(type.BaseType, target, name, args);
      }
    }
    catch (MissingMethodException)
    {
      // If we couldn't find the method, try on the base class
      if (type.BaseType != null)
      {
        return InvokeMemberOnType(type.BaseType, target, name, args);
      }
    }

    // Don't care if the method don't exist.
    return null;
  }
}

public static class PrivateReflectionDynamicObjectExtensions
{
  public static dynamic AsDynamic(this object o)
  {
    return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
  }
}