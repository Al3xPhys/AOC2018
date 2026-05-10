module AoC2018.Days.Day06

type Point = { X: int; Y: int; } // type Grid = Point array array


let parseInput (input:string) =
    input.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> line.Trim())

let manhattanDistance (p1: Point) (p2: Point) : (Point * int) =
    let distance = abs (p1.X - p2.X) + abs (p1.Y - p2.Y)
    p2, distance

let points (lines:string array) : Point array =
    lines
        |> Array.map (fun line ->
            let parts = line.Split(',') |> Array.map (fun s -> s.Trim())
            { X = int parts.[0]; Y = int parts.[1]; }
        )

let grid (lines:string array) : Point array = // create a grid of points covering the bounding box of the input points
    // find the bounding box of the points
    let minX = points lines |> Array.minBy (fun p -> p.X) |> fun p -> p.X
    let maxX = points lines |> Array.maxBy (fun p -> p.X) |> fun p -> p.X
    let minY = points lines |> Array.minBy (fun p -> p.Y) |> fun p -> p.Y
    let maxY = points lines |> Array.maxBy (fun p -> p.Y) |> fun p -> p.Y

    // create a grid of points covering the bounding box
    [| for y in minY .. maxY do
         for x in minX .. maxX do
            yield { X = x; Y = y; } |]

let getDistances (p: Point) (lines:string array) : (Point * int) array = // get the Manhattan distance from p to each of the input points
    points lines
    |> Array.map (fun point -> manhattanDistance p point)
    |> Array.sortBy snd

let shortestDistance (p: Point) (lines:string array) : (Point) option = // find the closest input point to p, and return it (or None if there is a tie)
    let distances = getDistances p lines
    if distances.Length > 1 && (snd distances.[0]) = (snd distances.[1]) then // if there is a tie for closest point, return None
        None // tie for closest point
    else
        let (closestPoint, _) = distances.[0]
        Some closestPoint // closest point

let part1 (lines:string array) : int =
    // Implement the logic for Part 1 here

    let closestPoints =
        grid lines
        |> Array.map (fun p -> p, shortestDistance p lines) // get the closest input point for each point in the grid
        |> Array.filter (fun (_, closest) -> closest.IsSome) // filter out points that have a tie for closest point
        |> Array.map (fun (p, closest) -> closest.Value) // get the closest input point for each point in the grid after filterting
        |> Array.groupBy id // group by closest input point and count how many points in the grid are closest to each input point
        |> Array.map (fun (point, group) -> point, group.Length) // get the count of points in the grid that are closest to each input point
        |> Array.sortByDescending snd // sort by count of closest points
    
    let largestArea = 
        closestPoints
        |> Array.maxBy (fun (point, count) -> count) // find the largest area among the closest points
        |> snd
        
    largestArea

let part2 (lines:string array) : int =
    // Implement the logic for Part 2 here

    let distances =
        grid lines
        |> Array.map (fun p -> p, getDistances p lines) // get the distances from each point in the grid to each input point
        |> Array.map (fun (p, distances) -> p, distances |> Array.sumBy snd) // sum the distances from each point in the grid to all input points
        |> Array.filter (fun (_, totalDistance) -> totalDistance < 10000) //
    
    let safeRegionSize = distances.Length // count how many points in the grid have a total distance to all input points of less than 10000
    safeRegionSize

let solvePart1 input =
    let parsed = parseInput input
    printfn "Part 1: %d" (part1 parsed)

let solvePart2 input =
    let parsed = parseInput input
    printfn "Part 2: %d" (part2 parsed)