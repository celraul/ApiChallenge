using Cel.GameOfLife.Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Cel.GameOfLife.API.RequestsExamples;

public class CreateBoardModelExample : IExamplesProvider<CreateBoardModel>
{
    public CreateBoardModel GetExamples()
    {
        return new CreateBoardModel
        {
            Name = "Sample test",
            BoardState = new()
            {
                new() { true, false, false },
                new() { false, true,  false },
                new() { false, false, true }
            }
        };
    }
}