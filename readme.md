# CommonCommons

CommonCommons is a collection of C# classes, utilities, static methods, and similar common utilities that can be used across different projects. This project aims to simplify the development process and reduce the amount of redundant code that developers often encounter.

## Installation
You can add CommonCommons to your project using NuGet Package Manager or by adding a reference to the CommonCommons.dll file.

## Usage
To use CommonCommons, simply add the appropriate using statement to your code file:

### Result<TValue, TError>
Using the MemberNotNullWhen attribute, we can allow for Nullability analysis of the IsSuccess property. This allows us to use the IsSuccess property in a null check without the compiler complaining about a possible null reference exception. 
Furthermore, we expose static methods to create Results in correct states, and expose Map and Match functions to compose this type.

```
Result<int, SomeErrorEnumOrMaybeEvenSomeString> result = SomeMethodThatReturnsResult();

if (result.IsSuccess)
{
    int value = result.Value;
    // Do something with the value
}
else
{
    SomeErrorEnumOrMaybeEvenSomeString error = result.Error;
    // Handle the error
}
```

## Contributing
Contributions to CommonCommons are always welcome! If you have an idea for a new class or utility, or if you have found a bug, please create an issue or pull request on GitHub.

## License
CommonCommons is licensed under the MIT License. See LICENSE for more information.