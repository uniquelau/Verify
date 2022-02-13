[TestFixture]
public class Tests
{
    [TestCase("Value1")]
    public Task UseFileNameWithParam(string arg)
    {
        return Verify(arg)
            .UseFileName("UseFileNameWithParam");
    }

    [TestCase("Value1")]
    public Task UseTextForParameters(string arg)
    {
        return Verify(arg)
            .UseTextForParameters("TextForParameter");
    }

    static IEnumerable<int> testCases = Enumerable.Range(0, 10000);

    [TestCaseSource(nameof(testCases))]
    public Task ManyTestCases(int testCase)
    {
        return Verify(testCase);
    }
}