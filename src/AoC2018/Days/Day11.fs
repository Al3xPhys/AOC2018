module AoC2018.Days.Day11

open System


type Cell() =
    member val PowerLevel = 0 with get, set

type Grid(gridSize: int) =
    // Initialize the grid with power levels based on the problem's formula
    member val Cells: Cell[,] = Array2D.init gridSize gridSize (fun _ _ -> Cell()) with get, set
    member val SumSquares: int64[,] = Array2D.zeroCreate<int64> gridSize gridSize

    member this.setPowerLevels(serialNumber: int) =
        this.Cells
        |> Array2D.iteri (fun x y cell ->
            let rackID = (x + 1) + 10
            let powerLevel = (rackID * (y + 1) + serialNumber) * rackID
            let hundredsDigit = (powerLevel / 100) % 10
            cell.PowerLevel <- hundredsDigit - 5)

    member this.printGrid(x, y, size) =
        [ y .. y + size - 1 ]
        |> List.map (fun y ->
            [ x .. x + size - 1 ]
            |> List.map (fun x -> sprintf "%3d" this.Cells.[x, y].PowerLevel)
            |> String.concat " ")
        |> String.concat "\n"
        |> printfn "%s"

    member this.getPowerArea x y areaSize =
        [ 0 .. areaSize - 1 ]
        |> List.collect (fun dy ->
            [ 0 .. areaSize - 1 ]
            |> List.map (fun dx -> this.Cells.[x + dx, y + dy].PowerLevel))
        |> List.sum

    member this.getPowerAreaFast x y areaSize =
        let x2 = x + areaSize - 1
        let y2 = y + areaSize - 1
        // D - B - C + A
        match x, y with
        | 0, 0 -> this.SumSquares[x2, y2]
        | 0, _ -> this.SumSquares[x2, y2] - this.SumSquares[x2, y - 1]
        | _, 0 -> this.SumSquares[x2, y2] - this.SumSquares[x - 1, y2]
        | _ ->
            this.SumSquares[x2, y2]
            - this.SumSquares[x2, y - 1]
            - this.SumSquares[x - 1, y2]
            + this.SumSquares[x - 1, y - 1]

    member this.validTopLeftCorners areaSize : (int * int) list list =
        [ 0 .. Array2D.length2 this.Cells - areaSize ]
        |> List.map (fun y -> [ 0 .. Array2D.length1 this.Cells - areaSize ] |> List.map (fun x -> (x, y)))

    member this.getPowerAreas(areaSize) =
        // first, get all valid top-left corner coords
        let validTopLeftCornersFlattened = List.concat (this.validTopLeftCorners areaSize)

        let powerAreas =
            validTopLeftCornersFlattened
            |> List.map (fun (x, y) -> ((x + 1, y + 1), this.getPowerAreaFast x y areaSize))

        powerAreas

    member this.genSumSquares() =
        for y in 0 .. Array2D.length2 this.Cells - 1 do
            for x in 0 .. Array2D.length1 this.Cells - 1 do
                if x = 0 && y = 0 then
                    this.SumSquares[x, y] <- int64 this.Cells[x, y].PowerLevel
                elif x = 0 && y <> 0 then
                    this.SumSquares[x, y] <- int64 this.Cells[x, y].PowerLevel + this.SumSquares[x, y - 1]
                elif y = 0 && x <> 0 then
                    this.SumSquares[x, y] <- int64 this.Cells[x, y].PowerLevel + this.SumSquares[x - 1, y]
                else
                    this.SumSquares[x, y] <-
                        int64 this.Cells[x, y].PowerLevel
                        + this.SumSquares[x - 1, y]
                        + this.SumSquares[x, y - 1]
                        - this.SumSquares[x - 1, y - 1]

        this.SumSquares

    member this.getLargestPowerAreaSize() =
        [ 1..gridSize ]
        |> List.map (fun size ->
            let bestforSize = this.getPowerAreas size |> List.maxBy snd
            (size, bestforSize))
        |> List.maxBy (fun (size, ((x, y), power)) -> power)

let solvePart1 (input: string) =
    let grid: Grid = Grid(300)
    grid.setPowerLevels (int input)
    grid.genSumSquares () |> ignore // Precompute the sum squares for the entire grid
    let maxPowerArea = grid.getPowerAreas 3 |> List.maxBy snd
    printfn "Max power area: %A" maxPowerArea

let solvePart2 (input: string) =
    let grid = Grid(300)
    grid.setPowerLevels (int input)
    grid.genSumSquares () |> ignore // Precompute the sum squares for the entire grid

    grid.getLargestPowerAreaSize ()
    |> fun (size, ((x, y), power)) -> printfn "Largest power area: %d at (%d, %d) for size %d" power x y size
