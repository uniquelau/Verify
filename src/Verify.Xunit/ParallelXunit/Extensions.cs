using VerifyXunit;

static class Extensions
{
    public static ITestCollection Collection(this IXunitTestCase test)
    {
        return test.TestMethod.TestClass.TestCollection;
    }

    public static ITypeInfo? CollectionDefinition(this IXunitTestCase test)
    {
        return test.Collection().CollectionDefinition;
    }

    public static bool IsCollection(this IXunitTestCase test)
    {
        return test.CollectionDefinition() != null;
    }

    public static IEnumerable<IAttributeInfo> GetAttributes<T>(this IXunitTestCase test)
        where T : Attribute
    {
        return test.TestMethod.TestClass.Class.GetCustomAttributes(typeof(T));
    }

    public static bool HasAttributes<T>(this IXunitTestCase test)
        where T : Attribute
    {
        return test.GetAttributes<T>().Any();
    }

    public static bool IsExplicitCollection(this IXunitTestCase test)
    {
        return test.IsCollection() ||
               test.HasAttributes<CollectionAttribute>();
    }

    public static bool IsSerial(this IXunitTestCase test)
    {
        return test.HasAttributes<SerialAttribute>();
    }

    static PropertyInfo defaultMethodDisplayInfo = typeof(TestMethodTestCase)
        .GetProperty("DefaultMethodDisplay", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static TestMethodDisplay MethodDisplay(this IXunitTestCase testCase)
    {
        return (TestMethodDisplay) defaultMethodDisplayInfo.GetValue(testCase)!;
    }

    static PropertyInfo defaultMethodDisplayOptions = typeof(TestMethodTestCase)
        .GetProperty("DefaultMethodDisplayOptions", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static TestMethodDisplayOptions MethodDisplayOptions(this IXunitTestCase testCase)
    {
        return (TestMethodDisplayOptions) defaultMethodDisplayOptions.GetValue(testCase)!;
    }
}