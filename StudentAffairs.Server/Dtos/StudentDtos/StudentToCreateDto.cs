namespace StudentAffairs.Server.Dtos.StudentDtos;

public record StudentToCreateDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public int GradeLevel { get; set; }
    public int DepartmentId { get; set; }
}
