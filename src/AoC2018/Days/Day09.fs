module AoC2018.Days.Day09

open System
open System.Runtime.CompilerServices

type MarbleCircle() =
    let marbles = Collections.Generic.LinkedList<int>()
    let mutable currentMarble = Some(marbles.AddFirst(0)) // Start with the first marble (0) in the circle

    member this.InsertClockwise(value: int) =
        let next =
            if currentMarble.Value.Next <> null then
                currentMarble.Value.Next
            else
                marbles.First // get the next node, wrap around if at the end

        let prev = if next.Next <> null then next.Next else marbles.First // get the node after next, wrap around if at the end
        let newNode = marbles.AddAfter(next, value) // insert after the next node
        currentMarble <- Some newNode

    member this.RemoveSevenCounterClockwise() =
        let mutable nodeToRemove = currentMarble.Value

        for _ in 1..7 do
            nodeToRemove <-
                if nodeToRemove.Previous <> null then
                    nodeToRemove.Previous
                else
                    marbles.Last // move counter-clockwise, wrap around if at the beginning

        let nextNode =
            if nodeToRemove.Next <> null then
                nodeToRemove.Next
            else
                marbles.First
        // update current marble to the one immediately clockwise of the removed marble
        currentMarble <- Some nextNode

        let removedValue = nodeToRemove.Value
        marbles.Remove(nodeToRemove) // remove the node from the circle

        removedValue

    member this.PrintCircle() =
        let values = marbles |> Seq.map string |> String.concat " "
        printfn "Circle: %s" values
        printfn "Current Marble: %d" this.CurrentMarbleValue

    member this.CurrentMarbleValue =
        match currentMarble with
        | Some node -> node.Value
        | None -> failwith "No current marble"

let parseInput (input: string) =
    let stripped = input.Split([| " " |], System.StringSplitOptions.RemoveEmptyEntries)
    let players = int stripped.[0]
    let lastMarble = int (stripped.[6].TrimEnd('.'))
    (players, lastMarble)

let part1 (input: int * int) : int64 =
    let players, lastMarble = input

    let listOfPlayerScores: int64 array = Array.zeroCreate (players + 1) // index 0 will be unused, players are 1-indexed

    let circle = MarbleCircle()

    for i in 1..lastMarble do

        if i % 23 = 0 then
            let removedMarble = circle.RemoveSevenCounterClockwise()
            // Update the current player's score
            let currentPlayer = (i - 1) % players + 1 // determine current player

            let scoreToAdd = int64 removedMarble + int64 i

            listOfPlayerScores.[currentPlayer] <- listOfPlayerScores.[currentPlayer] + scoreToAdd

        else
            circle.InsertClockwise(i)

    // circle.PrintCircle() // Debug: print the circle after each insertion/removal

    let maxScore = listOfPlayerScores |> Array.max
    maxScore


let solvePart1 input =
    let gameInfo = parseInput input
    printfn "Part 1 solution: %d" (part1 gameInfo)


let solvePart2 input =
    let gameInfo = parseInput input
    let gameInfo2 = gameInfo |> fun (players, lastMarble) -> (players, lastMarble * 100) // For part 2, we need to multiply the last marble value by 100
    printfn "Part 2 solution: %d" (part1 gameInfo2)
