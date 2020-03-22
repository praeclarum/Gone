# Gone

[![Language: F#](https://img.shields.io/badge/language-fsharp-purple.svg)](https://fsharp.org/)
[![Language: C#](https://img.shields.io/badge/language-csharp-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Language: Go](https://img.shields.io/badge/language-go-blue.svg)](https://golang.org/)

Gone is the [GO](https://golang.org/) compiler for [.NET](https://dotnet.microsoft.com/).

Come watch it being built: https://twitch.tv/FrankKrueger

## Goal

Build a compiler to parse the Go Lang:

```go
package main

import "fmt"

func main() {
    fmt.Println("Hello, 世界")
}
```

## Building

```cmd
dotnet build
```

### Modifying the Grammar

If you modify [`Parser.jay`](Parser/Parser.jay), then please rebuild the parser with:

```cmd
make
```

### Output

After running the tests a file `Output.dll` is created on your desktop.

- `~/Desktop/Output.dll`

Create a `config` file - `Output.runtimeconfig.json`:

```json
{
  "runtimeOptions": {
    "tfm": "netcoreapp3.1",
    "framework": {
      "name": "Microsoft.NETCore.App",
      "version": "3.1.0"
    }
  }
}
```

Then run the following command to see the compilier working:

```cmd
dotnet ~/Desktop/Output.dll
Hello, 世界
```

## Show Notes

- [Show Notes](SHOWNOTES.md)

## Contact

Mostly @praeclarum.

- [Twitter](https://twitter.com/praeclarum)
- [Twitch](https://twitch.tv/FrankKrueger)
- [YouTube](https://www.youtube.com/channel/UCFqpk9svseHIrsvshWSbDag)