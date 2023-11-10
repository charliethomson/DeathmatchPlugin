# Deathmatch Plugin

## Intended use
This is built as a team deathmatch plugin, with player configurable loadouts, killstreaks, and a (mid) buy menu integration.

## Installation
NOTE: I'd recommend updating `dllinclude\CounterStrikeSharp.dll` to the exact version running on your server for maximum compatibility, the included dll is `v27`.

- `dotnet build`
- [Install CounterStrikeSharp](https://docs.cssharp.dev/)
- Create a directory named `DeathmatchPlugin` in `game/csgo/addons/counterstrikesharp/plugins`
- Place `bin/[Release|Debug]/net7.0/DeathmatchPlugin.dll` in the directory you just created
- profit?

## Configuration
Currently there is no configuration, if you'd like to add/update the source code you're welcome to do so. (contributions welcome 😀)

## Dependencies
- CounterStrikeSharp

## License
see [LICENSE.md](./LICENSE.md)