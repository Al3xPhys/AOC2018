module AoC2018.Days.Day04

type Record = { Id: int; Timestamp: System.DateTime; Action: string }
// type TimeStamp = { Year: int; Month: int; Day: int; Hour: int; Minute: int }
let parseInput (input:string) =
    input.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> line.Trim())

let parseRecord = fun (line:string) ->
    let event = line.Split([|'['; ']'|]) |> Array.map (fun s -> s.Trim()) |> Array.filter (fun s -> s.Length > 0)
    let timestamp = System.DateTime.Parse(event.[0])
    let action = event.[1]
    let Id = 
        if action.Contains("Guard") then
            let idString = action.Split(' ') |> Array.filter (fun s -> s.StartsWith("#")) |> Array.head
            int (idString.TrimStart('#'))
        else
            // if the action is not a guard shift, use the previous guards ID
            0
    { Id = Id; Timestamp = timestamp; Action = action }

let minuteCountsByGuard = System.Collections.Generic.Dictionary<int, int array>()

let part1 (lines:string array) : int =
    // order records by timestamp
    let orderedRecords = lines |> Array.map parseRecord |> Array.sortBy (fun r -> r.Timestamp)

    // Fill missing guard IDs for sleep/wake records using the current shift guard.
    let updatedRecords =
        orderedRecords
        |> Array.fold (fun (currentId, records) record ->
            let newId = if record.Id <> 0 then record.Id else currentId
            (newId, records @ [{ record with Id = newId }])
        ) (0, [])
        |> snd
        |> Array.ofList

    // Track frequency of each minute (0..59) slept per guard.

    let getGuardMinutes guardId =
        if minuteCountsByGuard.ContainsKey(guardId) then
            minuteCountsByGuard.[guardId]
        else
            let arr = Array.zeroCreate 60
            minuteCountsByGuard.[guardId] <- arr
            arr

    let _, _ =
        updatedRecords
        |> Array.fold (fun (currentGuardId, sleepStartMinute: int option) record ->
            if record.Action.Contains("Guard") then
                (record.Id, None)
            elif record.Action.Contains("falls asleep") then
                (currentGuardId, Some record.Timestamp.Minute)
            elif record.Action.Contains("wakes up") then
                match sleepStartMinute with
                | Some startMinute ->
                    let minutes = getGuardMinutes currentGuardId
                    for m in startMinute .. (record.Timestamp.Minute - 1) do
                        minutes.[m] <- minutes.[m] + 1
                    (currentGuardId, None)
                | None ->
                    (currentGuardId, None)
            else
                (currentGuardId, sleepStartMinute)
        ) (0, None)

    printfn "Minute counts by guard: %A" minuteCountsByGuard

    let sleepiestGuard =
        minuteCountsByGuard
        |> Seq.maxBy (fun kvp -> kvp.Value |> Array.sum)


    let sleepiestMinute, _ =
        sleepiestGuard.Value
        |> Array.indexed
        |> Array.maxBy snd

    sleepiestGuard.Key * sleepiestMinute

let part2 (lines:string array) : int =
    part1 lines |> ignore // populate minuteCountsByGuard with the data from part 1

    let guardsBestMinute = 
        minuteCountsByGuard
        |> Seq.map (fun kvp ->
            let guardId = kvp.Key
            let minutes = kvp.Value
            let bestMinute, count = minutes |> Array.indexed |> Array.maxBy snd
            (guardId, bestMinute, count)
        )
        |> Seq.maxBy (fun (_, _, count) -> count)
    
    printfn "Guard with most frequent sleep minute: %A" guardsBestMinute

    let guardId, bestMinute, _ = guardsBestMinute
    guardId * bestMinute


let solvePart1 input = 
    let parsed = parseInput input 
    printfn "Part 1 solution: %d" (part1 parsed)

let solvePart2 input = 
    let parsed = parseInput input
    printfn "Part 2 solution: %d" (part2 parsed)