 global using MudBlazor;

namespace StudentAffairs.Server.Components.Pages.StudentPages;

public partial class StudentDetails
{
    [Parameter]
    public int StudentId { get; set; }
    private bool isLoading = true;

    StudentToReturnDto student = new();
    protected override async Task OnInitializedAsync()
        => await LoadStudent();

    private async Task LoadStudent()
    {
        var spec = new StudentWithEnrollmentsAndDepartmentSpecification(StudentId);
        var studentdb = await _unitOfWork.Repository<Student>().GetByIdWithSpecificationAsync(spec);
        if (studentdb != null)
        {
            student = _mapper.Map<Student, StudentToReturnDto>(studentdb);
            isLoading = false;
        }
        else
            Snackbar.Add("Student not found.", MudBlazor.Severity.Error);
    }

    private Task ToggleAttendanceDialog(EnrollmentToReturnWithStudentDto enrollment)
    {
        var parameters = new DialogParameters<AttendanceDialog>
        {
            { x => x.SelectedEnrollment, enrollment }
        };

        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true
        };

        return DialogService.ShowAsync<AttendanceDialog>("Delete", parameters, options);
    }

    private void NavigateToEdit(int id) => NavigationManager.NavigateTo($"/update-student/{id}");
    private void NavigateToDelete(int id) => NavigationManager.NavigateTo($"/DeleteStudent/{id}");
}
