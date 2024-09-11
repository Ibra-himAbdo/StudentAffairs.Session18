namespace StudentAffairs.Server.Components.Pages.CoursePages;

public partial class CoursesGrid
{
    private IReadOnlyList<Course> courses = [];

    protected async override Task OnInitializedAsync()
    {
        var spec = new CourseWithDepartmentSpecification();
        courses = await _unitOfWork.Repository<Course>().GetAllWithSpecificationAsync(spec);
    }
    private void NavigateToEditPage(int Id) => NavigationManager.NavigateTo($"/update-course/{Id}");
    private void NavigateToDeletePage(int Id) => NavigationManager.NavigateTo($"/delete-course/{Id}");
}
