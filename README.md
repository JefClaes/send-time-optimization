# Send Time Optimization

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

    Hour 0 - Ratio = Percentage 100M (Weight = Percentage 20.0M)
    Hour 1 - Ratio = Percentage 0M (Weight = Percentage 20.0M)
    Hour 8 - Ratio = Percentage 100M (Weight = Percentage 60.0M)
