module AoC2018.Days.Day03

// https://adventofcode.com/2018/day/3

// type claim is a record that contains all coordinates of a claim.
type Claim = { Id: int; X1: int; Y1: int; Width: int; Height: int; X2: int; Y2: int }
type Coordinate = { X: int; Y: int }
let parseInput (input:string) =
    input.Split('\n')
    |> Array.map string

let parseClaim = fun (coordinate:string) ->
    let splitString = coordinate.Split([|'@'; ':'|]) |> Array.map (fun s -> s.Trim())
    // convert the split string into a Claim record
    let id = splitString.[0].TrimStart('#') |> int //remove the '#' from the ID and make it an int
    let position = splitString.[1].Split(',') |> Array.map int
    let size = splitString.[2].Split('x') |> Array.map int
    { Id = id; X1 = position.[0]; Y1 = position.[1]; Width = size.[0]; Height = size.[1]; X2 = position.[0] + size.[0]; Y2 = position.[1] + size.[1] }

let coordinateClaims = System.Collections.Generic.Dictionary<Coordinate, int>()

let part1 (coordinates:string array) = 
    for claim in coordinates |> Array.map parseClaim do
        for x in claim.X1 .. claim.X2 - 1 do
            for y in claim.Y1 .. claim.Y2 - 1 do
                let coord = { X = x; Y = y }
                if coordinateClaims.ContainsKey coord then
                    coordinateClaims.[coord] <- coordinateClaims.[coord] + 1
                else
                    coordinateClaims.[coord] <- 1

    // now we can count how many coordinates are covered by more than one claim
    let overlapCount = coordinateClaims.Values |> Seq.filter (fun count -> count > 1) |> Seq.length
    overlapCount

let part2 (coordinates:string array) : System.String = 
    part1 coordinates |> ignore // we need to populate the coordinateClaims dictionary first
    // Part 2 is to find the claim that does not overlap with any other claim, 
    // we can do this by checking if any of the coordinates of a claim are covered by more than one claim
    let nonOverlappingClaim = 
        coordinates 
        |> Array.map parseClaim
        |> Array.tryFind (fun claim ->
            let mutable overlaps = false
            for x in claim.X1 .. claim.X2 - 1 do
                for y in claim.Y1 .. claim.Y2 - 1 do
                    let coord = { X = x; Y = y }
                    if coordinateClaims.[coord] > 1 then
                        overlaps <- true
            not overlaps)

    match nonOverlappingClaim with
    | Some claim -> sprintf "%d" claim.Id
    | None -> ""


let solvePart1 input = 
    let parsed = parseInput input 
    printfn "Part 1 solution: %d" (part1 parsed)

let solvePart2 input = 
    let parsed = parseInput input
    printfn "Part 2 solution: %s" (part2 parsed)