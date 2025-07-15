---
applyTo: "**/*.cs"
description: "Clear, concise .NET coding conventions, based on: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions"
---

# Formatting
- Four spaces per indentation level; never tabs.  
- Allman brace style (opening and closing braces on their own lines).  
- Place `using` directives outside the namespace declaration.  
- Enforce the use of file-scoped namespace declarations in C# files.

# Layout
- One statement per line; one declaration per line.  
- Blank line between methods, properties, and regions.  
- Maximum line length 120 characters; break lines before binary operators.  

# Naming Conventions
- PascalCase for public types, methods, properties, events, and namespaces.  
- camelCase for local variables, parameters, and private fields; `_camelCase` only for backing private fields.  
- ALL_CAPS for `const` fields.  
- Prefix interfaces with `I`; avoid Hungarian notation.  

# Type Usage
- Prefer language keywords (`string`, `int`) over .NET type names.  
- Always use `var`.  
- Prefer `int`; use unsigned types only when necessary.  

# Exceptions & Resources
- Catch only specific, recoverable exception types; avoid empty `catch`.  
- Use the `using` statement (declarative form) for `IDisposable` instances.  

# Asynchronous Programming
- Apply `async`/`await` for I/O‑bound operations; consider `ConfigureAwait(false)` in libraries.  

# LINQ
- Choose meaningful variable names; place `where` before `orderby`; rename result properties to avoid name clashes.  

# Comments
- Place `//` single‑line comments on their own line above the code block.  
- Begin with a capital letter, end with a period.  
- Provide XML documentation comments for public APIs.  

# Additional Guidelines
- Invoke static members via the class name.  
- Enable code analysis and version style rules through .editorconfig.  
