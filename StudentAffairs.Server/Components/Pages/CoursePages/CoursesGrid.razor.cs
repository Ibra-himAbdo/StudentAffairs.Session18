namespace StudentAffairs.Server.Components.Pages.CoursePages;

public partial class CoursesGrid
{
    private IReadOnlyList<Course> courses = [];

    protected async override Task OnInitializedAsync()
    {
        var spec = new CourseWithDepartmentSpecification();
        courses = await _unitOfWork.Repository<Course>().GetAllWithSpecificationAsync(spec);
    }
}
