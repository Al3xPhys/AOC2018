module AoC2018.Days.Day07
open System

type Worker = { Id: int; mutable AvailableAt: int; mutable CurrentTask: char option }
let parseInput (input:string) =
    input.Split([|"Step "; " can begin."; "\n"|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> line.Trim())
    |> Array.map (fun line -> line.[line.Length - 1], line.[0]) // convert to array of tuples of characters
    |> Array.groupBy fst // group by the first character (the step that must be completed before the second step can begin)
    |> Array.map (fun (step, dependencies) -> step, dependencies |> Array.map snd)
    // find char that is a dependency but not a step, and add it to the array with no dependencies
    |> fun arr ->
        let allSteps = arr |> Array.map fst
        let allDependencies = arr |> Array.collect snd
        // add mising steps that are dependencies but not steps, with no dependencies but dont add duplicates
        let missingSteps = allDependencies |> Array.filter (fun dep -> not (Array.contains dep allSteps)) |> Array.distinct
        Array.append arr (missingSteps |> Array.map (fun step -> step, [||]))
        |> Array.sortBy fst // sort by step character


let returnAvailableSteps (lines:(char * char [])[]) : char array = // find steps that have no dependencies
    let availableSteps = 
        lines
        |> Array.filter (fun (step, dependencies) -> dependencies.Length = 0)
        |> Array.map fst
        |> Array.sort
    availableSteps

let getNextStep (lines:(char * char [])[]) : char =
    returnAvailableSteps lines |> Array.head

let updateSteps (completedStep:char) (lines:(char * char [])[])  = // remove the completed step from the dependencies of the other steps
    lines
    |> Array.map (fun (step, dependencies) ->
        let newDependencies = dependencies |> Array.filter (fun dep -> dep <> completedStep)
        step, newDependencies
    )
    |> Array.filter (fun (step, dependencies) -> step <> completedStep) // remove the step that was just completed


let part1 (steps:(char * char [])[]) : string =
    // Implement the logic for Part 1 here

    // while there are still steps to complete, find the available steps and complete the first one, then update the steps
    let mutable remainingSteps = steps
    let mutable order = ""
    while remainingSteps.Length > 0 do
        let availableSteps = returnAvailableSteps remainingSteps
        if availableSteps.Length = 0 then
            failwith "No available steps, but there are still remaining steps. This should not happen if the input is valid."
        let nextStep = getNextStep remainingSteps
        order <- order + string nextStep
        remainingSteps <- updateSteps nextStep remainingSteps
    order

let part2 (steps:(char * char [])[]) : int =
    // Implement the logic for Part 2 here
    
    let workers = [| for i in 1 .. 5 -> { Id = i; AvailableAt = 0; CurrentTask = None } |] // make 2 workers
    let mutable remainingSteps = steps
    let mutable time = 0

    // while there are still steps to complete or workers are still working, assign available steps to available workers and move time forward
    while remainingSteps.Length > 0 || workers |> Array.exists (fun w -> w.CurrentTask.IsSome) do
        // find available workers and available steps, and assign them to each other
        let availableWorkers = workers |> Array.filter (fun w -> w.CurrentTask.IsNone && w.AvailableAt <= time)
        let availableSteps = returnAvailableSteps remainingSteps
        // assign available steps to available workers, prioritizing steps in alphabetical order
        // filter available steps to only those that are not already assigned to a worker to avoid over-assigning steps
        let availableSteps = availableSteps |> Array.filter (fun step -> not (workers |> Array.exists (fun w -> w.CurrentTask = Some step)))
        // truncate the available workers and steps to the minimum of the two lengths to avoid over-assigning workers or steps
        let n = min availableWorkers.Length availableSteps.Length
        let workersToAssign = availableWorkers |> Array.truncate n
        let stepsToAssign = availableSteps |> Array.truncate n
        // assign steps to workers and calculate when they will be available again
        for worker, step in Array.zip workersToAssign stepsToAssign do
            worker.CurrentTask <- Some step
            worker.AvailableAt <- time + (int step - int 'A' + 1) + 60 // task duration is 60 + step letter value (A=1, B=2, etc.)
        // find the next time a worker will be available
        let nextAvailableTime = workers |> Array.filter (fun w -> w.CurrentTask.IsSome) |> Array.map (fun w -> w.AvailableAt) |> Array.min
        time <- nextAvailableTime // move time forward to the next available worker
        // mark workers as available if their task is completed
        for worker in workers do
            match worker.CurrentTask with
            | Some step when worker.AvailableAt <= time ->
                worker.CurrentTask <- None
                remainingSteps <- updateSteps step remainingSteps
            | _ -> ()
    time
     
let solvePart1 input =
    let lines = parseInput input
    let result = part1 lines
    printfn "Part 1: %s" result

let solvePart2 input =
    let lines = parseInput input
    let result = part2 lines
    printfn "Part 2: %d" result

