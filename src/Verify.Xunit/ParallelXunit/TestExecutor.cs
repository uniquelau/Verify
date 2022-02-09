class TestExecutor :
    XunitTestFrameworkExecutor
{
    public TestExecutor(AssemblyName assembly, ISourceInformationProvider provider, IMessageSink sink)
        : base(assembly, provider, sink)
    {
    }

    protected override async void RunTestCases(IEnumerable<IXunitTestCase> tests, IMessageSink messageSink, ITestFrameworkExecutionOptions ptions)
    {
        var newTest = tests.Select(DeriveTestCase);
        using var runner = new XunitTestAssemblyRunner(TestAssembly, newTest, DiagnosticMessageSink, messageSink, ptions);
        await runner.RunAsync().ConfigureAwait(true);
    }

    // By default, test cases in a test class share the same collection instance which ensures they run synchronously.
    // By providing a unique test collection instance to every test case in a test class you can make them all run in parallel.
    IXunitTestCase DeriveTestCase(IXunitTestCase test)
    {
        if (test.Method.ReturnType.Name=="System.Void" ||
            test.IsSerial() ||
            test.IsExplicitCollection())
        {
            return test;
        }


        return DuplicateTest(test);
    }

    IXunitTestCase DuplicateTest(IXunitTestCase test)
    {
        var collection = CreateTestCollection(test);
        // Duplicate the test and assign it to the new collection
        var newTestClass = new TestClass(collection, test.TestMethod.TestClass.Class);
        var newTestMethod = new TestMethod(newTestClass, test.TestMethod.Method);
        var options = test.MethodDisplayOptions();
        var display = test.MethodDisplay();

        if (test is XunitTheoryTestCase)
        {
            return new XunitTheoryTestCase(DiagnosticMessageSink, display, options, newTestMethod);
        }

        if (test is XunitTestCase)
        {
            return new XunitTestCase(DiagnosticMessageSink, display, options, newTestMethod, test.TestMethodArguments);
        }

        throw new($"Test case {test.GetType()} not supported");
    }

    static TestCollection CreateTestCollection(IXunitTestCase test)
    {
        var collection = test.Collection();

        // Create a new collection with a unique id for the test case.
        return new(
            collection.TestAssembly,
            collection.CollectionDefinition,
            displayName: $"{collection.DisplayName} {collection.UniqueID}")
        {
            UniqueID = Guid.NewGuid()
        };
    }
}