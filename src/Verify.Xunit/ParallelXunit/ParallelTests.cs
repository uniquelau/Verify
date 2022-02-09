namespace VerifyXunit;

public class ParallelTests :
    XunitTestFramework
{
    public ParallelTests(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
    {
        return new TestExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }
}