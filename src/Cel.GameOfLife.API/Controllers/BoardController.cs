using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using Cel.GameOfLife.API.Attributes;
using Cel.GameOfLife.API.Models;
using Cel.GameOfLife.API.RequestsExamples;
using Cel.GameOfLife.Application.Models;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.CreateBoard;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.ResetBoard;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Queries.GetNextState;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Cel.GameOfLife.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardController(IAppMediator appMediator) : BaseController
{
    private readonly IAppMediator _appMediator = appMediator;

    /// <summary>
    /// It creates a new board setting the initial state.
    /// </summary>
    /// <param name="board"></param>
    /// <returns>Id of board</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(CreateBoardModel), typeof(CreateBoardModelExample))]
    public async Task<ActionResult> Post([FromBody] CreateBoardModel board)
    {
        var result = await _appMediator.Send(new CreateBoardCommand(board));
        return HandleResult(result);
    }

    /// <summary>
    /// It generates and returns a next state for the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <returns>Returns board with the next state.</returns>
    [HttpPut("{id}/next")]
    [ProducesResponseType(typeof(ApiResponse<BoardModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Put(string id)
    {
        var result = await _appMediator.Send(new GenerateNextStateCommand(id));
        return HandleResult(result);
    }

    /// <summary>
    /// It generates and returns a next state for the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <param name="count">Count to generete next states.</param>
    /// <returns>Returns board with the next state.</returns>
    [HttpPut("{id}/next/{count}")]
    [ProducesResponseType(typeof(ApiResponse<BoardModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Put(string id, int count)
    {
        var result = await _appMediator.Send(new GenerateNextStateCommand(id, count));
        return HandleResult(result);
    }

    /// <summary>
    /// It reset to initial state.
    /// </summary>
    /// <param name="id">Id of board</param>
    [HttpPut("{id}/reset")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Reset(string id)
    {
        var result = await _appMediator.Send(new ResetBoardCommand(id));
        return HandleResult(result);
    }

    /// <summary>
    /// It returns the last state of the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <returns>last state of board</returns>
    [CacheResponse]
    [HttpGet("{id}/finalState")]
    [ProducesResponseType(typeof(ApiResponse<bool[][]>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetFinalState(string id)
    {
        var result = await _appMediator.Query(new GetFinalStateQuery(id));
        return HandleResult(result);
    }
}
