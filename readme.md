# CommonCommons

CommonCommons is a collection of C# classes, utilities, static methods, and similar common utilities that can be used
across different projects. This project aims to simplify the development process and reduce the amount of redundant code
that developers often encounter.

## Installation

You can add CommonCommons to your project using NuGet Package Manager or by adding a reference to the CommonCommons.dll
file. See it within the NuGet site with the link below:

https://www.nuget.org/packages/CommonCommons/

## Usage

To use CommonCommons, simply refer to any of the types or functions we expose, such as:

- The Result<TValue, TError> class, which is a Result style monad that uses null analysis attributes to maintain null
  safety and assist the C# compiler in knowing when the Value or Error fields are not null
- The NullableExtensions static class, which are a series of extension methods that allow you to compose and use
  nullable types (T? or Nullable<T>) like a Maybe monad

## Contributing

Contributions to CommonCommons are always welcome! If you have an idea for a new class or utility, or if you have found
a bug, please create an issue or pull request on GitHub.

## License

CommonCommons is licensed under the MIT License. See LICENSE for more information.
