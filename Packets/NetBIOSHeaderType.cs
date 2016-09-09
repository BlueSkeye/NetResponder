namespace NetResponder.Packets
{
    internal enum NetBIOSHeaderType
    {
        DirectUnique = 10,
        DirectGroup = 11,
        Broadcast = 12,
        Error = 13,
        RequestQuery = 14,
        PositiveQueryResponse = 15,
        NegativeQueryResponse = 16
    }
}
