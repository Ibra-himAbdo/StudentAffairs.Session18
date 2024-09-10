namespace StudentAffairs.Server.Components.Pages.ProfessorPages;

public partial class CreateProfessor
{
    private AddProfessorDto professorModel = new();
    private IReadOnlyList<Department> departments = [];
    bool success = false;

    private async Task OnValidSubmit()
    {
        success = true;
        await SaveProfessor(professorModel);
        Snackbar.Add("Professor information submitted successfully!", MudBlazor.Severity.Success);
        professorModel = new();
        success = false;
        StateHasChanged();
    }

    private async Task SaveProfessor(AddProfessorDto professorToCreateDto)
    {
        var professor = _mapper.Map<AddProfessorDto, Professor>(professorToCreateDto);

        var isProfessorExist = await _unitOfWork.GetProfessorRepository().IsProfessorExist(professor);
        if (isProfessorExist)
        {
            Snackbar.Add("Professor already exists!", MudBlazor.Severity.Error);
            return;
        }

        await _unitOfWork.Repository<Professor>().AddAsync(professor);
        await _unitOfWork.CompleteAsync();
    }

    protected async override Task OnInitializedAsync()
        => departments = await _unitOfWork.Repository<Department>().GetAllAsync();
}
