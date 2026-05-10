# Advent of Code 2018 (F#)

Learning-focused Advent of Code 2018 solutions in F#.

## Goals

- Practice functional-first thinking in F#
- Improve parsing, collections, and recursion skills
- Keep solutions readable and easy to revisit

## Project Layout

- `src/AoC2018` main puzzle runner and daily solutions
- `src/Common` shared console project
- `tests` test project folder (currently empty)

## Run a Day

From the repository root:

```bash
dotnet run --project src/AoC2018/AoC2018.fsproj -- <day> <part>
```

Examples:

```bash
dotnet run --project src/AoC2018/AoC2018.fsproj -- 1 1
dotnet run --project src/AoC2018/AoC2018.fsproj -- 4 2
```

## Progress

| Day | Part 1 | Part 2 |
|-----|--------|--------|
| 01  | Done   | Done   |
| 02  | Done   | Done   |
| 03  | Done   | Done   |
| 04  | Done   | Done   |
| 05+ | Done   | Done   |

## Notes

- Personal Advent of Code inputs are intentionally ignored by git.
- Build artifacts (`bin`, `obj`) and editor local files are ignored.
