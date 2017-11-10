namespace AcademicProgressTracker.Models
{
    public class Classification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }
}