# Using the analyzers during development

## Working with analyzers in Visual Studio or another IDE

- Output of the analyzers will show up as entries of various levels (i.e. Errors, Warnings, Messages) in the Error List window of Visual Studio for the currently open files (the error codes will link to more info on the violations). You'll also see squiggly lines in the code editor as it is usual for any code issues.
- For a lot of issues you'll be able to use automatic code fixes, or suppress them if they're wrong in the given context from the Visual Studio Quick Actions menu (<kbd>Ctrl</kbd> + <kbd>.</kbd> by default). The [Productivity Power Tools](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.ProductivityPowerPack2017) Visual Studio extension can remove and order namespace imports, as well as format the document when saving (just enable its "Format document on save" option). [CodeMaid](https://marketplace.visualstudio.com/items?itemName=SteveCadwallader.CodeMaid) can similarly format whole projects or even the full solution at once. The command-line [`dotnet-format`](https://github.com/dotnet/format) can fix all issues that can be fixed automatically.
- The _Build.props_ (for .NET Framework projects, _NetFx.Build.props_) file disables analyzers during Visual Studio build, not to slow down development; you can enable them by setting `RunAnalyzersDuringBuild` to `true`. After this, they'll show for the whole solution after a rebuild (but not when you build an already built solution, just with a rebuild or a fresh build).
- When working on reducing cognitive complexity to pass the [S3776 rule](https://rules.sonarsource.com/csharp/RSPEC-3776/) you can make use of the [CognitiveComplexity plugin for JetBrains Rider](https://plugins.jetbrains.com/plugin/12024-cognitivecomplexity) or the [ReSharper plugin of the same name](https://plugins.jetbrains.com/plugin/12391-cognitivecomplexity) for Visual Studio. It annotates the individual contributors to the cognitive complexity score so you can quickly identify trouble spots with the least effort. The scoring algorithm is not 100% identical to the one used in Sonar but it's similar enough to greatly speed up the task of refactoring complex methods.
- The following VS extensions fit nicely into an analyzer-aided developer workflow:
  - [Rewrap](https://marketplace.visualstudio.com/items?itemName=stkb.Rewrap-18980) helps you wrap comment text at a character limit you can change in the settings. We recommend this to be set to 120.
  - [Editor Guidelines](https://marketplace.visualstudio.com/items?itemName=PaulHarrington.EditorGuidelines) displays the column guidelines defined in our editor config.

## Practices on suppressing a rule for a given piece of code

This section is about suppressing analyzers for just certain lines of code or at most a class. On configuring analyzers for a wider scope, see [Configuring analyzers](ConfiguringAnalyzers.md)

Analyzers are not perfect so they can give false positives, and there can always be justified exceptions to every rule, so suppressing analyzer warnings (as e.g. Visual Studio offers it, with and `SuppressMessage` attribute or with `#pragma warning disable`) is fine if done in moderation (if you have to do it a lot for a given rule then the rule is not suitable for your coding style). When doing so adhere to the following:

- Always suppress a warning in the smallest scope possible.
- Use the `#pragma` suppress for specific lines of code.
- Only use the `SuppressMessage` attribute on a member (or even a whole project) if the suppress absolutely must cover the whole member or if it's for a method argument.
- Always add a justification.

Example where the `SuppressMessage` attribute is suitable (note how the suppression affects the whole type, and a justification is provided):

```c#
[SuppressMessage(
    "Design",
    "CA1008:Enums should have zero value",
    Justification = "Since members correspond to specific numbers, we can't have a zero value.")]
public enum Numbers
{
    One = 1,
    Two = 2,
    Three = 3,
}
```

Example where the `#pragma` suppress is suitable (note how the suppression only affects the class declaration and a justifications is provided):

```c#
    // The extension method should follow the naming of the original class.
#pragma warning disable S101 // Types should be named in PascalCase
    public static class RNGCryptoServiceProviderExtensions
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static int Next(this RNGCryptoServiceProvider rng, int minValue, int maxValue)
        {
            // ...
        }
    }
```

For further details see the [official docs](https://docs.microsoft.com/en-us/visualstudio/code-quality/in-source-suppression-overview).
