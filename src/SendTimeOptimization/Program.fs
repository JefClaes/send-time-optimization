namespace SendTimeOptimization

open System
open STOModule

module program =

    [<EntryPoint>]
    let main argv = 
        let events = 
            [
                Event.Sent (new DateTime(2015, 6, 1, 0, 10, 10), "Mail_1");
                Event.Sent (new DateTime(2015, 6, 1, 1, 10, 10), "Mail_2");
                Event.ClickedThrough (new DateTime(2015, 6, 1, 0, 10, 10), "Mail_1");
                Event.Sent (new DateTime(2015, 6, 1, 8, 0, 0), "Mail_3");
                Event.Sent (new DateTime(2015, 6, 1, 8, 30, 0), "Mail_4");
                Event.Sent (new DateTime(2015, 6, 1, 8, 45, 0), "Mail_5");
                Event.ClickedThrough (new DateTime(2015, 6, 1, 9, 1, 0), "Mail_3");
                Event.ClickedThrough (new DateTime(2015, 6, 1, 9, 2, 0), "Mail_4");
                Event.ClickedThrough (new DateTime(2015, 6, 1, 9, 3, 0), "Mail_5");                    
            ] 

        let dbth = mailsFrom events |> distributionByTheHour

        for h in dbth do 
            printfn "Hour %u - Ratio = %A (Weight = %A)" h.Hour h.Ratio h.Weight

        Console.ReadLine() |> ignore

        0 // return an integer exit code
