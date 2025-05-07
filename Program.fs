open System
open System.IO
open RandomGenerator
open PoinFitter


[<EntryPoint>]
let main argv =
    // Der Pfad zur CSV-Datei – passe ihn ggf. an
    let csvPath = @"C:\Users\duets\Projekts\F#Projekt\MeinFSharpProjekt\data.csv"

    // Führe die Plot-Funktion aus
    plotFromCSV csvPath

    // Rückgabewert für den Exit-Code
    0

(* [<EntryPoint>]
let main argv =
    let path = @"C:\Users\duets\Projekts\F#Projekt\MeinFSharpProjekt\data.csv"
    let numPoints = 10 // Anzahl der Punkte, die generiert werden sollen
  
    printfn "Generiere neue Zufallsdaten..."
    generateAndSave path numPoints
    printfn "Datei erstellt: %s" path

    0 *)

