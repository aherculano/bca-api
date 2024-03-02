﻿namespace Domain.Models.Auction.ValueObjects;

public class Bid : ValueObject
{
    public Bid(string bidderName,
        decimal bidValue)
    {
        BidderName = bidderName;
        BidValue = bidValue;
    }

    public string BidderName { get; protected set; }

    public decimal BidValue { get; protected set; }
}