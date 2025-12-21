namespace CleanArchitecture.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public DateRange(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date must be after start date");
        
        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;
}