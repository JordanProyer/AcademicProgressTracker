using System;

namespace AcademicProgressTracker.Models.Graphs
{
    public class KnnResult
    {
        public int UserId { get; set; }
        public double Distance { get; set; }
        public String Label { get; set; }
        public double PredictedModuleMark { get; set; }
        public double MarkAfterXCourseworks { get; set; }
        public double CircleSize { get; set; }
    }
}