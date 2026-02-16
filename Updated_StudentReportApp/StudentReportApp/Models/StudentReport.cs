namespace StudentReportApp.Models
{
    public class StudentReport
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        
        public List<string> SubjectNames { get; set; } = new List<string>();

        public List<double> StudentGrades { get; set; } = new List<double>();

        public string GetCurrentDateTime() => DateTime.Now.ToString("MMMM dd, yyyy - hh:mm tt");

        public double ComputeGPA()
        {
            if (StudentGrades == null || !StudentGrades.Any()) return 0.0;
            return Math.Round(StudentGrades.Average(), 2);
        }

        public string GetRemarks(double gpa)
        {
            return gpa <= 3.0 ? "Passed" : "Failed";
        }
    }
}