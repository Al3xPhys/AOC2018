module AoC2018.Days.Day10

open System

type Point() =
    member val Coordinates = (0, 0) with get, set // (x, y) coordinates of the point
    member val Velocity = (0, 0) with get, set // (vx, vy) velocity of the point
    // Method to update the point's position based on its velocity
    member this.move(timeStep: int) =
        let (x, y) = this.Coordinates
        let (vx, vy) = this.Velocity
        this.Coordinates <- (x + vx * timeStep, y + vy * timeStep)

type PointGrid() =
    member val Points: Point list = [] with get, set // List of points in the grid
    member val CurrentGrid = [| [| ' ' |] |] with get, set // 2D array representing the grid

    member this.AddPoint(point: Point) = this.Points <- point :: this.Points

    member this.GetBounds() =
        let xs = this.Points |> List.map (fun p -> fst p.Coordinates)
        let ys = this.Points |> List.map (fun p -> snd p.Coordinates)
        let minX = List.min xs
        let maxX = List.max xs
        let minY = List.min ys
        let maxY = List.max ys
        (minX, maxX, minY, maxY)

    member this.GetGridArea() =
        let (minX, maxX, minY, maxY) = this.GetBounds()
        let area: int64 = abs (int64 (maxX - minX + 1) * int64 (maxY - minY + 1))
        area

    member this.makeGrid() =
        let (minX, maxX, minY, maxY) = this.GetBounds()
        this.CurrentGrid <- Array.init (maxY - minY + 1) (fun _ -> Array.init (maxX - minX + 1) (fun _ -> ' '))
        // Place points on the grid
        this.Points
        |> List.iter (fun p ->
            let (x, y) = p.Coordinates
            this.CurrentGrid.[y - minY].[x - minX] <- '*')

    member this.UpdatePoints(timeStep: int) =
        // Update the position of each point based on its velocity
        this.Points <-
            this.Points
            |> List.map (fun (p: Point) ->
                p.move (timeStep)
                p)

    member this.PrintGrid() =
        // print the grid
        this.CurrentGrid |> Array.iter (fun row -> printfn "%s" (String.Concat row))

let parseInput (input: string) =
    // Placeholder for input parsing logic

    let pointGrid = PointGrid()

    let findMatches =
        Text.RegularExpressions.Regex.Matches(
            input,
            "position=<\\s*(-?\\d+),\\s*(-?\\d+)> velocity=<\\s*(-?\\d+),\\s*(-?\\d+)>"
        )
        |> Seq.cast<Text.RegularExpressions.Match>
        |> Seq.map (fun m ->
            let x = int m.Groups.[1].Value
            let y = int m.Groups.[2].Value
            let vx = int m.Groups.[3].Value
            let vy = int m.Groups.[4].Value
            let point = Point()
            point.Coordinates <- (x, y)
            point.Velocity <- (vx, vy)
            point)
        |> Seq.iter pointGrid.AddPoint

    pointGrid

let part1 (input: string) =
    let parsedInput = parseInput input

    let findMinimumArea () =
        let rec loop previousArea timeStep =
            parsedInput.UpdatePoints 1
            let newArea = parsedInput.GetGridArea()

            if newArea < previousArea then
                loop newArea (timeStep + 1)
            else
                timeStep // Return the time step just before the area started increasing

        loop (parsedInput.GetGridArea()) 0

    let timeOfMinimumArea = findMinimumArea ()
    printfn "Time of minimum area: %d" timeOfMinimumArea

    // At this point, the points have just started to spread out again, so we need to move back one step to get the message
    parsedInput.UpdatePoints -1
    printfn "Final grid:"
    parsedInput.makeGrid ()
    parsedInput.PrintGrid()

let solvePart1 (input: string) : int =
    // Placeholder for Part 1 solution logic
    part1 input
    0

let solvePart2 (input: string) : int =
    part1 input
    0
