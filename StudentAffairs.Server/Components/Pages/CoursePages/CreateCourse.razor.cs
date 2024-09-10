namespace StudentAffairs.Server.Components.Pages.CoursePages;

public partial class CreateCourse
{
    private AddCourseDto courseModel = new();
    private IReadOnlyList<Department> departments = [];
    bool success = false;

    private async Task OnValidSubmit()
    {
        success = true;
        await SaveCourse(courseModel);
        Snackbar.Add("Course information submitted successfully!", MudBlazor.Severity.Success);
        courseModel = new();
        success = false;
        StateHasChanged();
    }

    private async Task SaveCourse(AddCourseDto createdCourse)
    {
        var course = _mapper.Map<AddCourseDto, Course>(createdCourse);

        var isCourseExist = await _unitOfWork.GetCourseRepository().IsCourseExist(course);
        if (isCourseExist)
        {
            Snackbar.Add("Course already exists!", MudBlazor.Severity.Error);
            return;
        }

        await _unitOfWork.Repository<Course>().AddAsync(course);
        await _unitOfWork.CompleteAsync();
    }

    protected async override Task OnInitializedAsync()
        => departments = await _unitOfWork.Repository<Department>().GetAllAsync();
}
