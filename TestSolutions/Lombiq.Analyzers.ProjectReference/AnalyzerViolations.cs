namespace Lombiq.Analyzers.ProjectReference;

// This file has intentional analyzer violations. Only edit the one in the TestSolutions directory. Any copy in the
// subdirectories will be copied over during build. Copying is necessary so the source files are in a directory with an
// existing parent .editorconfig file. (Linking will cause them to physically be outside of the .editorconfig's area of
// effect.) Also please commit the updated version everywhere, because if the file is not present before the build
// starts then MSBuild won't include it despite the <Copy> task. For this purpose the <Copy> task is only there to
// ensure that the changes propagate and get committed.
public class Class1
{
    private int Number;
    public Class1(int number)
    {
        Number = number;
    }
}
