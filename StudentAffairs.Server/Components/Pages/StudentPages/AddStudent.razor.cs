namespace StudentAffairs.Server.Components.Pages.StudentPages;

public partial class AddStudent
{
    private StudentToCreateDto studentModel = new();
    private IReadOnlyList<Department> departments = new List<Department>();
    bool success = false;
    private async Task OnValidSubmit()
    {
        try
        {
            await SaveStudent(studentModel);
            Snackbar.Add("Student information submitted successfully!", MudBlazor.Severity.Success);
            studentModel = new(); // Reset form model
            success = true;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An error occurred: {ex.Message}", MudBlazor.Severity.Error);
            success = false;
        }
    }

    private async Task SaveStudent(StudentToCreateDto createdStudent)
    {
        var student = _mapper.Map<Student>(createdStudent);

        var IsStudentExist = await _unitOfWork.GetStudentRepository().IsStudentExist(student);
        if (IsStudentExist)
        {
            Snackbar.Add("Student already exist!", MudBlazor.Severity.Error);
            return;
        }

        await _unitOfWork.Repository<Student>().AddAsync(student);
        await _unitOfWork.CompleteAsync();
    }

    protected async override Task OnInitializedAsync()
        => departments = await _unitOfWork.Repository<Department>().GetAllAsync();
}
