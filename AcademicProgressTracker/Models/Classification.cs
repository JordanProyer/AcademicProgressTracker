namespace AcademicProgressTracker.Models
{
    public class Classification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
    }
}