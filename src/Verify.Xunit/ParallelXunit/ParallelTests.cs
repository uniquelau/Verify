namespace VerifyXunit;

public sealed class ParallelTests :
    XunitTestFramework
{
    public ParallelTests(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
    {
        return new CustomTestExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }
}