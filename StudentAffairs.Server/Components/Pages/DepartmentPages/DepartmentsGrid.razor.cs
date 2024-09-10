namespace StudentAffairs.Server.Components.Pages.DepartmentPages;

public partial class DepartmentsGrid
{
    private IReadOnlyList<Department> departments = [];

    protected async override Task OnInitializedAsync()
    => departments = await _unitOfWork.Repository<Department>().GetAllAsync();
}
