module PoinFitter

open System
open System.IO
open Plotly.NET
open FSharp.Data

// Typinferenz durch eingebettete Beispieldaten (nicht reale Datei!)
type DataCsv = CsvProvider<"X,Y\n1.0,2.0\n2.0,3.9">

// Mittelwert-Funktion
let mean (xs: float list) = List.sum xs / float (List.length xs)

// Lineare Regression
let linearRegression (data: (float * float) list) =
    let xs, ys = List.unzip data
    let xMean, yMean = mean xs, mean ys
    let numerator = List.sum (List.map2 (fun x y -> (x - xMean) * (y - yMean)) xs ys)
    let denominator = List.sumBy (fun x -> (x - xMean) ** 2.0) xs
    let m = numerator / denominator
    let b = yMean - m * xMean
    (m, b) 

// CSV einlesen, Regression berechnen, Plot erzeugen
let plotFromCSV (csvPath: string) =
    let csv = DataCsv.Load(csvPath)

    let data =
        csv.Rows
        |> Seq.map (fun row -> float row.X, float row.Y)
        |> List.ofSeq

    let xs, ys = List.unzip data
    let m, b = linearRegression data
    let linearFit = xs |> List.map (fun x -> m * x + b)

    let points = Chart.Point(xs, ys, Name = "Originaldaten")
    let line = Chart.Line(xs, linearFit, Name = "Lineare Regression")

    let chart =
        [ points; line ]
        |> Chart.combine
        |> Chart.withTitle "CSV-Daten mit Linearer Regression"
        |> Chart.withXAxisStyle "X"
        |> Chart.withYAxisStyle "Y"

    let outputPath = "plot_from_csv.html"
    chart |> Chart.saveHtmlAs outputPath
    printfn $"Plot gespeichert unter: {Path.GetFullPath(outputPath)}"
