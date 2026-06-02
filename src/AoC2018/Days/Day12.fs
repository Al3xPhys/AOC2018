module AoC2018.Days.Day12

open System

type State(potString: string, ZeroIndex: int) =
    member val Pots = potString with get, set
    member val ZeroIndex = ZeroIndex with get, set // Represents the state of the pots, where Pots is a string of '.' and '#' characters, and ZeroIndex indicates the index of the pot that corresponds to index 0.

let calculateScore (state: State) =
    // printfn "Calculating score for state: %s with zero index at %d" state.Pots state.ZeroIndex // print the current state and zero index for debugging
    // the score is the sum of the indicies of plant-containing pots (i.e. pots with '#') minus the zero index
    state.Pots
    |> Seq.mapi (fun i c -> if c = '#' then Some(i - state.ZeroIndex) else None) // get the index of each pot, but only if it contains a plant
    |> Seq.choose id // filter out the None values
    |> Seq.sum // sum the remaining values to get the final score
    |> int64

let nextGeneration (rulesMap: Map<string, string>, state: State) =
    // We need to consider centre pot and two left and two right of it.
    // Need to pad the state with '.' on both sides to account for growth. Need to add 4 '.' on both sides to account for growth in the next generation.
    // or is it 2 '.' on both sides? Let's start with 4 to be safe.
    let paddedState = "...." + state.Pots + "...."

    //after padding, the zero index is now at index 4 because of the 4 added to the left
    let newZeroIndex = state.ZeroIndex + 4


    // now we can apply the rules to generate the next state
    // need to iterate over padde state 5 character at a time, find if matches any rules and build the new state string
    let newStateString =
        [ 0 .. paddedState.Length - 5 ] // iterate over the padded state, taking 5 characters at a time
        |> List.map (fun i -> // pipe the indices to a function that will apply the rules
            let pattern = paddedState.Substring(i, 5) // get the 5 character pattern starting at index i

            match rulesMap |> Map.tryFind pattern with // find the rule that matches the pattern
            // some rules will not match which means do nothing, some rules with match and give '#' but some will give '.', need to handle both cases
            | Some(result) -> result // will give the result of the rule, which is either '#' or '.'
            | None -> ".") // if no rule matches, the result is '.' by default
        |> String.concat ""

    // now we have the new state string, we need to trim the leading and trailing '.' characters and adjust the zero index accordingly
    let trimmedStateString = newStateString.Trim('.')
    let leadingDots = newStateString.Length - newStateString.TrimStart('.').Length
    let finalZeroIndex = newZeroIndex - leadingDots - 2
    state.Pots <- trimmedStateString
    state.ZeroIndex <- finalZeroIndex

let runGenerations (rulesMap: Map<string, string>, state: State, generations: int64, extrapGeneration: int64) =
    let mutable currentScore: int64 = calculateScore state
    let mutable currentScoreDiff: int64 = 0
    let mutable stableCount: int64 = 0

    [ 1L .. generations ]
    |> List.iter (fun gen ->
        nextGeneration (rulesMap, state)
        // if score doesnt change, reached stable state, can stop early
        let newScore: int64 = calculateScore state
        let scoreDiff = newScore - currentScore

        if scoreDiff = currentScoreDiff then
            stableCount <- stableCount + 1L

        if stableCount > 100 then
            printfn "Extrapolating final score: %d" (calculateScore state + (extrapGeneration - gen) * scoreDiff)
            // stop iterating by throwing an exception that we catch outside the loop
            raise (Exception("Stable state reached"))
        else
            currentScore <- calculateScore state
            currentScoreDiff <- scoreDiff)

let parseInput (input: string) =
    let lines = input.Split([| '\n'; '\r' |], StringSplitOptions.RemoveEmptyEntries)

    let initialState = lines.[0].Replace("initial state: ", "").Trim()

    let rules =
        lines.[1..]
        |> Seq.map (fun line -> line.Split(" => "))
        |> Seq.map (fun parts -> (parts.[0], parts.[1]))

    (initialState, rules)

let solvePart1 input =
    let (initialState: string), (rules) = parseInput input
    let state: State = State(initialState, 0)
    let rulesMap = rules |> Map.ofSeq

    // printfn "Initial state: %s" state.Pots

    runGenerations (rulesMap, state, 1000, 50000000000L) // run for a large number of generations to see if it stabilizes



let solvePart2 input = 0
