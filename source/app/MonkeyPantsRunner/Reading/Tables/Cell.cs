namespace MonkeyPants.Reading.Tables
{
    public class Cell
    {
        public string Value { get; private set; }

        public static Cell Empty
        {
            get { return new Cell(null); }
        }

        public Cell(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value ?? "<null>";
        }

        public bool IsEmpty()
        {
            return Value == null || string.Empty.Equals(Value);
        }
    }
}