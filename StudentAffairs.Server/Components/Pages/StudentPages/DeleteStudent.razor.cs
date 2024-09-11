namespace StudentAffairs.Server.Components.Pages.StudentPages;

public partial class DeleteStudent
{
    [Parameter]
    public int StudentId { get; set; }
    private bool isLoading = true;

    Student? _student;
    protected override async Task OnInitializedAsync()
        => await LoadStudent();
    private async Task LoadStudent()
    {
        _student = await _unitOfWork.Repository<Student>().GetByIdAsync(StudentId);
        if (_student is null)
        {
            Snackbar.Add("Student not found.", MudBlazor.Severity.Error);
            return;
        }

        isLoading = false;

    }

    private async Task DeleteStudentAsync()
    {
        if (_student is null)
        {
            Snackbar.Add("Student not found.", MudBlazor.Severity.Error);
            return;
        }

        _unitOfWork.Repository<Student>().Delete(_student);
        await _unitOfWork.CompleteAsync();

        Snackbar.Add("Student deleted successfully.", MudBlazor.Severity.Success);
        await Task.Delay(TimeSpan.FromSeconds(2));
        NavigationManager.NavigateTo("/Students");
    }
}
