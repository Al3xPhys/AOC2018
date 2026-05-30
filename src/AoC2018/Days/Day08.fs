module AoC2018.Days.Day08

open System



let parseInput (input: string) =
    input.Split([| " " |], System.StringSplitOptions.RemoveEmptyEntries)

let getTotalMetadata (input: string[]) =
    // read first two numbers, first number is number of child nodes, second number is number of metadata entries
    // if first number is 0, then read the metadata and add to total.
    // if first number is not 0, then recursively read the child nodes and add their metadata to the total, then read the metadata and add to total
    let mutable totalMetadata = 0
    let mutable index = 0

    let rec readNode () =
        let numChildNodes = int input.[index]
        let numMetadataEntries = int input.[index + 1]
        index <- index + 2

        for i in 1..numChildNodes do
            readNode ()

        for i in 1..numMetadataEntries do
            totalMetadata <- totalMetadata + int input.[index]
            index <- index + 1

    readNode ()
    totalMetadata

let findRootNodeValue (input: string[]) =
    let mutable totalMetadata = 0
    let mutable index = 0

    let rec readNode () =
        let numChildNodes = int input.[index]
        let numMetadataEntries = int input.[index + 1]
        let mutable childNodeValues = []

        index <- index + 2

        for i in 1..numChildNodes do
            childNodeValues <- childNodeValues @ [ readNode () ]

        let mutable nodeMetaTotal = 0

        for i in 1..numMetadataEntries do
            if numChildNodes = 0 then
                nodeMetaTotal <- nodeMetaTotal + int input.[index]
            else
                let metaEntry = int input.[index]

                if metaEntry > 0 && metaEntry <= childNodeValues.Length then
                    nodeMetaTotal <- nodeMetaTotal + childNodeValues.[metaEntry - 1]

            index <- index + 1

        nodeMetaTotal

    readNode ()

let part1 (input: string[]) : int =
    let totalMetadata = getTotalMetadata input
    totalMetadata

let part2 (input: string[]) : int =
    let rootNodeValue = findRootNodeValue input
    rootNodeValue

let solvePart1 input =
    let lines = parseInput input
    let result = part1 lines
    printfn "Part 1: %d" result

let solvePart2 input =
    let lines = parseInput input
    let result = part2 lines
    printfn "Part 2: %d" result
