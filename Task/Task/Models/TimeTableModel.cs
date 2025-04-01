namespace Task.Models
{
    public class TimetableInputModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalHours { get; set; }
        public List<SubjectModel> Subjects { get; set; } = new List<SubjectModel>();
    }

    public class SubjectModel
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }

}
