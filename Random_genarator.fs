module RandomGenerator

open System
open System.IO

let random = Random()

let generateData numLines =
    [ for i in 1..numLines do
        let x = float i
        let y = (random.NextDouble() * 10.0) + 2.0
        yield (x, y) ]

let saveCsv (filePath: string) (data: (float * float) list) =
    use writer = new StreamWriter(filePath)
    writer.WriteLine("X,Y")
    data |> List.iter (fun (x, y) -> writer.WriteLine(sprintf "%f,%f" x y))

let generateAndSave path =
    let data = generateData 100
    saveCsv path data

