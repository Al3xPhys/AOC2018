open AoC2018.Days
open System.IO

[<EntryPoint>]
let main argv =
    //argv is two elements, 1st is the day number, 2nd is the part number
    match argv with
    | [|"1"; "1"|] ->
        Day01.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day01.txt")) |> ignore
    | [|"1"; "2"|] ->
        Day01.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day01.txt")) |> ignore
    | [|"2"; "1"|] ->
        Day02.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day02.txt")) |> ignore
    | [|"2"; "2"|] ->
        Day02.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day02.txt")) |> ignore
    | [|"3"; "1"|] ->
        Day03.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day03.txt")) |> ignore
    | [|"3"; "2"|] ->
        Day03.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day03.txt")) |> ignore
    | [|"4"; "1"|] ->
        Day04.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day04.txt")) |> ignore
    | [|"4"; "2"|] ->
        Day04.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day04.txt")) |> ignore
    | [|"5"; "1"|] ->
        Day05.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day05.txt")) |> ignore
    | [|"5"; "2"|] ->
        Day05.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day05.txt")) |> ignore
    | [|"6"; "1"|] ->
        Day06.solvePart1 (File.ReadAllText("src/AoC2018/Inputs/day06.txt")) |> ignore
    | [|"6"; "2"|] ->
        Day06.solvePart2 (File.ReadAllText("src/AoC2018/Inputs/day06.txt")) |> ignore
    | _ ->
        printfn "Please specify a day and part to solve (e.g., '1 2' for Day 1, Part 2)."
    0 // return an integer exit code