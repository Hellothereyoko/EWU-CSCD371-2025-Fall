namespace Logger
{
    public record FullName
    {
        public string First { get; init; }
        public string Last { get; init; }
        public string? Middle { get; init; }

        public FullName(string first, string last, string? middle = null)
        {
            First = first;
            Last = last;
            Middle = middle;
        }
    }
}

