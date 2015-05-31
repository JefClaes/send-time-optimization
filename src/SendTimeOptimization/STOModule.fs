namespace SendTimeOptimization

open System

module STOModule =

    type Event =
        | Sent of Timestamp : DateTime * MailId : string
        | ClickedThrough of Timestamp : DateTime * MailId : string

    type Mail = { SentTimestamp : DateTime; MailId : string; ClickedThrough : bool }

    type ClickToSendRatio = | Percentage of decimal
        with
            static member From all withClickthrough =               
                match all with
                | 0 -> ClickToSendRatio.Percentage 0M
                | _ -> ClickToSendRatio.Percentage ( ( decimal withClickthrough / decimal all ) * decimal 100 )

    type HourWeight = | Percentage of decimal
        with
            static member From mailsInHour mailsInDay =
                match mailsInDay with
                | 0 -> HourWeight.Percentage 0M
                | _ -> HourWeight.Percentage ( ( decimal mailsInHour / decimal mailsInDay ) * decimal 100 )
                
    type DistributionByTheHour = { Hour : int; Weight : HourWeight; Ratio : ClickToSendRatio }

    let mailsFrom events = 
        let folder state e =
            match e with
            | Event.Sent ( ts, id ) -> { MailId = id; SentTimestamp = ts; ClickedThrough = false } :: state
            | Event.ClickedThrough ( ts , id ) -> 
            (
                match state |> List.tryFind(fun x -> x.MailId = id) with
                | Some m -> { m with ClickedThrough = true } :: ( state |> List.filter(fun x -> x.MailId <> id) )
                | None -> state
            )
        Seq.fold folder [] events

    let distributionByTheHour mails = 
        mails 
        |> Seq.groupBy (fun x -> x.SentTimestamp.Hour) 
        |> Seq.map 
            (fun ( h, mailsInHour ) -> 
            (
                let mailsInDayCnt = mails |> Seq.length
                let mailsInHourCnt = mailsInHour |> Seq.length
                let mailsWithClickThroughInHourCnt = mailsInHour |> Seq.where (fun x -> x.ClickedThrough) |> Seq.length

                { 
                    Hour = h
                    Weight = HourWeight.From mailsInHourCnt mailsInDayCnt
                    Ratio = ClickToSendRatio.From mailsInHourCnt mailsWithClickThroughInHourCnt 
                }
            ))
        |> Seq.sortBy (fun x -> x.Hour)