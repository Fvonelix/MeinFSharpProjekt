open Plotly.NET

// Beispiel-Daten
let data = [ (1.0, 2.0); (2.0, 3.5); (3.0, 5.0); (4.0, 6.5) ]

// Mittelwert-Funktion
let mean xs = List.sum xs / float (List.length xs)

// Lineare Regression
let linearRegression data =
    let xs, ys = List.unzip data
    let xMean, yMean = mean xs, mean ys
    let numerator = List.sum (List.map2 (fun x y -> (x - xMean) * (y - yMean)) xs ys)
    let denominator = List.sumBy (fun x -> (x - xMean) ** 2.0) xs
    let m = numerator / denominator
    let b = yMean - m * xMean
    (m, b)

// Quadratische Regression
let quadraticRegression data =
    let xs, ys = List.unzip data
    let design =
        xs |> List.map (fun x -> [| x * x; x; 1.0 |]) |> array2D
    let X = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.DenseOfArray(design)
    let y = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(Array.ofList ys)
    let coeffs = (X.TransposeThisAndMultiply(X)).Inverse() * X.TransposeThisAndMultiply(y)
    let a, b, c = coeffs.[0], coeffs.[1], coeffs.[2]
    (a, b, c)

// Plot erstellen
[<EntryPoint>]
let main _ =
    let xs, ys = List.unzip data

    // Lineare Regressionswerte berechnen
    let m, b = linearRegression data
    let linearFit = xs |> List.map (fun x -> m * x + b)

    // Quadratische Regressionswerte berechnen
    let a, b2, c = quadraticRegression data
    let quadraticFit = xs |> List.map (fun x -> a * x * x + b2 * x + c)

    // Streudiagramm der Originaldaten
    let points = Chart.Point(xs, ys, Name="Datenpunkte")

    // Lineare Fit-Kurve
    let lineFit = Chart.Line(xs, linearFit, Name="Lineare Regression")

    // Quadratische Fit-Kurve
    let quadFit: GenericChart.GenericChart = Chart.Line(xs, quadraticFit, Name="Quadratische Regression")

    // Plot kombinieren
    let chart =
        [ points; lineFit; quadFit ]
        |> Chart.combine
        |> Chart.withTitle "Regression von Datenpunkten"
        |> Chart.withXAxisStyle "x"
        |> Chart.withYAxisStyle "y"

    // Diagramm als HTML speichern
    chart |> Chart.saveHtmlAs "regression_plot.html"


    printfn "Plot wurde als HTML gespeichert. Öffne die Datei 'regression_plot.html' im Browser."

    0
