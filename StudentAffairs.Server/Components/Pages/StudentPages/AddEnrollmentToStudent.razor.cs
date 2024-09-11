namespace StudentAffairs.Server.Components.Pages.StudentPages;

public partial class AddEnrollmentToStudent
{

    [Parameter] public int StudentId { get; set; }

    private bool _isAdding = true;
    private bool _isLoading = true;
    private Student? _student;
    private Dictionary<int, int> _enrollments = new();
    private IReadOnlyList<Course> _courses = new List<Course>();
    private HashSet<int> _selectedIds = new();
    private const int MaxSelectableButtons = 5;

    protected override async Task OnInitializedAsync()
    {
        await LoadStudentData();
        await LoadCoursesData();
    }

    private async Task LoadStudentData()
    {
        var studentSpec = new StudentWithEnrollmentsAndDepartmentSpecification(StudentId);
        _student = await _unitOfWork.Repository<Student>().GetByIdWithSpecificationAsync(studentSpec);

        if (_student is null)
        {
            Snackbar.Add("Student not found.", MudBlazor.Severity.Error);
            return;
        }

        _isLoading = false;

        if (_student.Enrollments?.Any() == true)
        {
            _isAdding = false;
            _enrollments = _student.Enrollments.ToDictionary(e => e.Id, e => e.CourseId);
            _selectedIds = new HashSet<int>(_enrollments.Values);
        }
    }

    private async Task LoadCoursesData()
    {
        var spec = new CourseWithDepartmentSpecification(new CourseSpecificationParams());
        _courses = await _unitOfWork.Repository<Course>().GetAllWithSpecificationAsync(spec);
    }

    private void ToggleButton(int courseId)
    {
        if (_selectedIds.Contains(courseId))
        {
            _selectedIds.Remove(courseId);
            var enrollmentId = _enrollments.FirstOrDefault(e => e.Value == courseId).Key;
            if (enrollmentId != 0) _enrollments[enrollmentId] = 0;
        }
        else if (_selectedIds.Count < MaxSelectableButtons)
        {
            _selectedIds.Add(courseId);
            var emptyEnrollmentId = _enrollments.FirstOrDefault(e => e.Value == 0).Key;
            if (emptyEnrollmentId != 0) _enrollments[emptyEnrollmentId] = courseId;
        }
    }

    private bool IsButtonDisabled(int buttonId) =>
        !_selectedIds.Contains(buttonId) && _selectedIds.Count >= MaxSelectableButtons;

    private async Task Submit()
    {
        try
        {
            string message = string.Empty;
            if (_isAdding)
                await AddStudentEnrollments();
            else
                message = await UpdateStudentEnrollments();

            Snackbar.Add($"Enrollments {(_isAdding ? "added" : message)}", MudBlazor.Severity.Success);
            await Task.Delay(TimeSpan.FromSeconds(2));

            if (_isAdding) NavigateToStudentDetails(StudentId);
            else NavigateToStudentGrade(StudentId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to {(_isAdding ? "add" : "update")} enrollments.", MudBlazor.Severity.Error);
            Console.WriteLine(ex.Message);
        }
    }

    private async Task AddStudentEnrollments()
    {
        var enrollments = _selectedIds.Select(id => new Enrollment
        {
            StudentId = _student!.Id,
            CourseId = id,
            EnrollmentDate = DateTime.UtcNow.Date
        }).ToList();
        await _unitOfWork.Repository<Enrollment>().AddRangeAsync(enrollments);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<string> UpdateStudentEnrollments()
    {
        var existingEnrollments = _student!.Enrollments.ToList();

        var newEnrollments = _selectedIds
            .Where(id => !existingEnrollments.Any(e => e.CourseId == id))
            .Select(id => new Enrollment
            {
                StudentId = _student.Id,
                CourseId = id,
                EnrollmentDate = DateTime.UtcNow.Date
            }
            ).ToList();

        var updatedEnrollments = _enrollments
            .Where(e => e.Value != 0) // Exclude enrollments marked for removal
            .Select(e => new Enrollment
            {
                Id = e.Key,
                StudentId = _student.Id,
                CourseId = e.Value,
                EnrollmentDate = DateTime.UtcNow.Date
            }
            ).ToList();

        // Find enrollments that actually need updating
        var enrollmentsToUpdate = updatedEnrollments
            .Where(ue => !existingEnrollments.Any(ee =>
                ee.Id == ue.Id &&
                ee.CourseId == ue.CourseId &&
                ee.StudentId == ue.StudentId))
            .ToList();

        var changesRequired = newEnrollments.Any() || enrollmentsToUpdate.Any();

        if (changesRequired)
        {
            if (enrollmentsToUpdate.Any())
                _unitOfWork.Repository<Enrollment>().UpdateRange(enrollmentsToUpdate);

            if (newEnrollments.Any())
                await _unitOfWork.Repository<Enrollment>().AddRangeAsync(newEnrollments);

            await _unitOfWork.CompleteAsync();
        }
        return changesRequired ? "updated" : "No changes";
    }

    private bool AreEnrollmentChangesRequired(
    IEnumerable<Enrollment> updatedEnrollments,
    IEnumerable<Enrollment> existingEnrollments)
    {
        var updatedEnrollmentKeys = updatedEnrollments.Select(e => new { e.StudentId, e.CourseId });
        var existingEnrollmentKeys = existingEnrollments.Select(e => new { e.StudentId, e.CourseId });

        return updatedEnrollmentKeys.Except(existingEnrollmentKeys).Any();
    }

    private void NavigateToStudentGrade(int id) => NavigationManager.NavigateTo($"/StudentGrade/{id}");
    private void NavigateToStudentDetails(int id) => NavigationManager.NavigateTo($"/StudentDetails/{id}");
}
