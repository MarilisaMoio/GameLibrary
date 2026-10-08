# 🎮 GameLibrary

A simple console application to manage a personal video game collection, written in C# while learning the language.

## Features

- **See** all games in the library
- **Add** a game, either:
  - **Digital** — with PEGI rating and platform
  - **Physical** — with condition
- **Remove** a game by its index
- Input validation: invalid choices are rejected and asked again

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## What I practiced

- Abstract classes and inheritance
- `virtual` / `override` and `base` calls
- Enums with explicit values and validation (`Enum.TryParse`, `Enum.IsDefined`)
- Generic methods with constraints (`where T : struct, Enum`)
- Properties with `init` and validated setters (`field` keyword)
- Input loops with `do` / `while`

## What I want to achieve next

- [x] Refactor the code to achieve better readability
- [x] Edit a game
- [x] Add the price for a single game and implement a method to retrieve the total for the collection
- [ ] Add the enum for genres
- [ ] Add the possibility to see the games filtered by platform, genre and type
- [ ] Save the collection to a JSON file and use it on startup
- [ ] Add the possibility to exit from an action 
