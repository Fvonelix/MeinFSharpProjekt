
open System
open System.IO
open Plotly.NET
open FSharp.Data
open RandomGenerator


// CSV-Datei-Typ definieren (Pfad egal, nur für Typinferenz)
type DataCsv = CsvProvider<"data.csv">

// Mittelwert-Funktion für float-Werte
let mean (xs: float list) = List.sum xs / float (List.length xs)

// Lineare Regression mit float-Daten
let linearRegression (data: (float * float) list) =
    let xs, ys = List.unzip data
    let xMean, yMean = mean xs, mean ys
    let numerator = List.sum (List.map2 (fun x y -> (x - xMean) * (y - yMean)) xs ys)
    let denominator = List.sumBy (fun x -> (x - xMean) ** 2.0) xs
    let m = numerator / denominator
    let b = yMean - m * xMean
    (m, b)

// Plot-Funktion, die CSV einliest und alles erstellt
let plotFromCSV (csvPath: string) =
    let csv = DataCsv.Load(csvPath)

    // Konvertiere decimal-Werte zu float!
    let data =
        csv.Rows
        |> Seq.map (fun row -> (float row.X, float row.Y))
        |> List.ofSeq

    let xs, ys = List.unzip data
    let m, b = linearRegression data
    let linearFit = xs |> List.map (fun x -> m * x + b)

    let points = Chart.Point(xs, ys, Name="Originaldaten")
    let line = Chart.Line(xs, linearFit, Name="Lineare Regression")

    let chart =
        [ points; line ]
        |> Chart.combine
        |> Chart.withTitle "CSV-Daten mit Linearer Regression"
        |> Chart.withXAxisStyle "X"
        |> Chart.withYAxisStyle "Y"

    let outputPath = "plot_from_csv.html"
    chart |> Chart.saveHtmlAs outputPath
    printfn $"Plot gespeichert unter: {Path.GetFullPath(outputPath)}"

// Einstiegspunkt
[<EntryPoint>]
let main argv =
    let path = @"C:\Users\duets\Projekts\F#Projekt\MeinFSharpProjekt\data.csv"
    if File.Exists path then
        plotFromCSV path
    else
        printfn "Datei 'data.csv' wurde nicht gefunden."
    0
(* [<EntryPoint>]
let main argv =
    let path = @"C:\Users\duets\Projekts\F#Projekt\MeinFSharpProjekt\data.csv"

    if not (File.Exists path) then
        printfn "Generiere neue Zufallsdaten..."
        generateAndSave path
        printfn "Datei erstellt: %s" path
    else
        printfn "Datei existiert bereits: %s" path
    0
 *)
