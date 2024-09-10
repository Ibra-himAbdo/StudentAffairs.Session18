namespace StudentAffairs.Server.Components.Pages.DepartmentPages;

public partial class CreateDepartment
{
    private AddDepartmentDto departmentModel = new();
    bool success = false;

    private async Task OnValidSubmit()
    {
        success = true;
        await SaveDepartment(departmentModel);
        Snackbar.Add("Department added successfully!", MudBlazor.Severity.Success);
        departmentModel = new();  // Reset the form model
        success = false;
        StateHasChanged();
    }

    private async Task SaveDepartment(AddDepartmentDto departmentToCreateDto)
    {
        var department = _mapper.Map<AddDepartmentDto, Department>(departmentToCreateDto);

        var result = await _unitOfWork.GetDepartmentRepository().IsDepartmentExist(department);
        if (result)
        {
            Snackbar.Add("Department already exists!", MudBlazor.Severity.Error);
            return;
        }

        await _unitOfWork.Repository<Department>().AddAsync(department);
        await _unitOfWork.CompleteAsync();
    }
}
