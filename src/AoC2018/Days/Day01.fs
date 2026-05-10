module AoC2018.Days.Day01

// https://adventofcode.com/2018/day/1
let parseInput (input:string) =
    input.Split('\n')
    |> Array.map int

let part1 (numbers:int array) = 
    numbers |> Array.sum

let part2 (numbers:int array) : int option = 
    if numbers.Length = 0 then
        None
    else
        // Part 2 must repeatedly apply changes until a frequency is seen twice.
        let seen = System.Collections.Generic.HashSet<int>()
        seen.Add(0) |> ignore

        let rec loop index currentFrequency =
            let nextFrequency = currentFrequency + numbers.[index]
            if seen.Contains(nextFrequency) then
                Some(nextFrequency)
            else
                seen.Add(nextFrequency) |> ignore
                let nextIndex = (index + 1) % numbers.Length
                loop nextIndex nextFrequency

        loop 0 0

let solvePart1 input = 
    let parsed = parseInput input
    let result = part1 parsed
    printfn "Part 1: %d" result

let solvePart2 input = 
    let parsed = parseInput input
    let result = part2 parsed
    printfn "Part 2: %A" result
    if result.IsSome then
        printfn "Part 2: %d" result.Value
    else
        printfn "Part 2: No duplicate frequency found"