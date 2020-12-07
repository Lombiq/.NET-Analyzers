# Using the analyzers during development



## Working with analyzers in Visual Studio or another IDE

Output of the analyzers will show up as entries of various levels (i.e. Errors, Warnings, Messages) in the Error List window of Visual Studio for the currently open files (the error codes will link to more info on the violations). You'll also see squiggly lines in the code editor as it is usual for any code issues. For a lot of issues you'll be able to use automatic code fixes, or suppress them if they're wrong in the given context from the Quick Actions menu (<kbd>Ctrl</kbd> + <kbd>.</kbd> by default).

The *Build.props* file disables analyzers during Visual Studio build, not to slow down development; you can enable them by setting `RunAnalyzersDuringBuild` to `true`. After this, they'll show for the whole solution after a rebuild (but not when you build an already built solution, just with a rebuild or a fresh build).

Note that if you have the [Microsoft Code Analysis Visual Studio extension](https://docs.microsoft.com/en-us/visualstudio/code-quality/install-fxcop-analyzers#vsix) installed then it'll clash with the analyzer packages and you'll see warnings in Visual Studio of the like of "An instance of analyzer Microsoft.NetCore.CSharp.Analyzers.Runtime.CSharpDoNotRaiseReservedExceptionTypesAnalyzer cannot be created from..." To fix this, disable or uninstall the extension.

When working on reducing cognitive complexity to pass the [S3776 rule](https://rules.sonarsource.com/csharp/RSPEC-3776) you can make use of the [CognitiveComplexity plugin for JetBrains Rider](https://plugins.jetbrains.com/plugin/12024-cognitivecomplexity) or the [ReSharper plugin of the same name](https://plugins.jetbrains.com/plugin/12391-cognitivecomplexity) for Visual Studio. It annotates the individual contributors to the cognitive complexity score so you can quickly identify trouble spots with the least effort. The scoring algorithm is not 100% identical to the one used in Sonar but it's similar enough to greatly speed up the task of refactoring complex methods.


## Practices on suppressing a rule for a given piece of code

Analyzers are not perfect so they can give false positives, and there can always be justified exceptions to every rule, so suppressing analyzer warnings is fine if done in moderation (if you have to do it a lot for a given rule then the rule is not suitable for your coding style). When doing so adhere to the following:

- Always suppress a warning in the smallest scope possible.
- Use the `#pragma` suppress for specific lines of code.
- Only use the `SuppressMessage` attribute on a member (or even a whole project) if the suppress absolutely must cover the whole member or if it's for a method argument.
- Always add a justification.
