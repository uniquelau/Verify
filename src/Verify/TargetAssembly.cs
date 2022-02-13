﻿static class TargetAssembly
{
    static Assembly? assembly;
    public static string ProjectDir { get; private set; } = null!;
    public static string? SolutionDir { get; private set; }

    public static void Assign(Assembly assembly)
    {
        if (TargetAssembly.assembly != null)
        {
            return;
        }

        Namer.UseAssembly(assembly);
        ProjectDir = AttributeReader.GetProjectDirectory(assembly);
#pragma warning disable CS4014
        FileCache.Init(ProjectDir);
#pragma warning restore CS4014
        AttributeReader.TryGetSolutionDirectory(assembly, out var solutionDir);
        SolutionDir  = solutionDir;
        ApplyScrubbers.UseAssembly(solutionDir, ProjectDir);
        TargetAssembly.assembly = assembly;
    }
}