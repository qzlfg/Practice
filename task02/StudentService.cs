public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    =>  _students.Where(student => string.Equals(student.Faculty, faculty, StringComparison.OrdinalIgnoreCase));


    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade) 
    => _students.Where(s => s.Grades.Any() && s.Grades.Average() >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(student => student.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(student => student.Faculty);

    public string GetFacultyWithHighestAverageGrade()
        => _students
        .GroupBy(student => student.Faculty)
        .MaxBy(g => g.SelectMany(s => s.Grades).Any() ? g.SelectMany(s => s.Grades).Average() : 0)
        ?.Key;
}