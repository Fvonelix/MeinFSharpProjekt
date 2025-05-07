module RandomGenerator

open System
open System.IO
open System.Globalization

let random = Random()

let generateData numPoints =
    [ for i in 1..numPoints do
        let x = float i
        // y liegt zwischen x-1.00 und x+1.00
        let y = (random.NextDouble() * 2.0) + (x - 1.0)  // (random.NextDouble() * 2) gibt einen Wert zwischen 0 und 2 zurück
        yield (x, y) ]

let saveCsv (filePath: string) (data: (float * float) list) =
    // Datei wird hier überschrieben (nicht angehängt!)
    use writer = new StreamWriter(filePath, append = false)
    writer.WriteLine("X,Y")
    // Verwende hier das amerikanische Kulturformat (Punkt als Dezimaltrennzeichen)
    let cultureInfo = CultureInfo("en-US")
    for (x, y) in data do
        // Formatierte Zahl mit Tausendertrennzeichen und Punkt als Dezimaltrennzeichen
        let formattedX = x.ToString("N2", cultureInfo)  // "N2" für 2 Dezimalstellen
        let formattedY = y.ToString("N2", cultureInfo)  // "N2" für 2 Dezimalstellen
        writer.WriteLine($"{formattedX},{formattedY}")

let generateAndSave (filePath: string) (numPoints: int) =
    let data = generateData numPoints
    saveCsv filePath data
