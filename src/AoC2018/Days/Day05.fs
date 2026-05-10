module AoC2018.Days.Day05

open System

let parseInput (input:string) =
    input.Trim()

let part1 (input:string) : int =
    // Implement the logic for Part 1 here
    let reactedPolymer = 
        input.ToCharArray()
        |> Array.fold (fun (stack: char list) c ->
            match stack with
            | prev :: rest when Char.ToLower(prev) = Char.ToLower(c) && prev <> c ->
                rest // when previous character is the same letter but different case, pop it from the stack
            | _ -> c :: stack // otherwise, push the current character onto the stack
        ) []
    reactedPolymer.Length

let part2 (input:string) : int =
    // Implement the logic for Part 2 here


    // make array of all unit types (a-z)
    let unitTypes = [|'a' .. 'z'|]

    (* for each unit type, remove all instances of that type (both cases) and react the polymer, 
    then find the length of the resulting polymer *)

    let shortestLength =
        unitTypes
        |> Array.map (fun unitType ->
            let modifiedPolymer =
                input.ToCharArray()
                |> Array.filter (fun c -> Char.ToLower(c) <> unitType)
                |> String.Concat
            part1 modifiedPolymer // react the modified polymer and get its length
        ) 
        |> Array.min // find the minimum length among all modified polymers

    shortestLength

let solvePart1 input =
    let parsed = parseInput input
    printfn "Part 1: %d" (part1 parsed)

let solvePart2 input =
    let parsed = parseInput input
    printfn "Part 2: %d" (part2 parsed)