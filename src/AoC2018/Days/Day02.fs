module AoC2018.Days.Day02

let parseInput (input:string) =
    input.Split('\n')
    |> Array.map string


let part1 (ids:string array) = 
    let mutable twos = 0 // count of strings with a letter that appears exactly twice
    let mutable threes = 0 // count of strings with a letter that appears exactly three times
    for str in ids do // for each string, count the occurrences of each character
        let counted =
            str 
            |> Seq.countBy id 
            |> Seq.filter (fun (_, count) -> count = 2 || count = 3)
            // now check if we have a 2 and a 3
            |> Seq.map snd // we only care about the counts, not the characters
            // we want to know if we have a 2 and a 3, so we can use a set
            |> Set.ofSeq
        if counted.Contains(2) then twos <- twos + 1
        if counted.Contains(3) then threes <- threes + 1
    twos * threes

let part2 (ids:string array) : System.String = 
    let mutable result = "No solution found"
    for str in ids do
        for other in ids do
            if str <> other then
                let zippedStrings =
                    str 
                    |> Seq.zip other
                    |> Seq.filter (fun (c1,c2) -> c1 <> c2 )
                if Seq.length zippedStrings = 1 then
                    let commonString = 
                        str
                        |> Seq.zip other
                        |> Seq.filter (fun (c1,c2) -> c1 = c2 )
                        |> Seq.map fst
                        |> Seq.toArray
                        |> System.String
                    result <- commonString
    result
let solvePart1 input =
    let ids = parseInput input
    let result = part1 ids
    printfn "Part 1 solution: %d" result


let solvePart2 input =
    let ids = parseInput input
    let result = part2 ids
    printfn "Part 2 solution: %s" result