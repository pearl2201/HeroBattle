namespace MasterServer.Domain.Mics
{
    public class SelectOption<T>
    {
        public string Text { get; set; }

        public T Label { get; set; }

        public SelectOption(string label, T value)
        {
            Text = label;
            Label = value;
        }
    }

    public class CategorySelectOption<T>
    {
        public string Category { get; set; }
        public string Label { get; set; }

        public T Value { get; set; }

        public CategorySelectOption(string category, string label, T value)
        {
            Category = category;
            Label = label;
            Value = value;
        }
    }
}
