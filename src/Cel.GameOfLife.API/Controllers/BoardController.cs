using Cel.GameOfLife.API.Models;
using Cel.GameOfLife.API.RequestsExamples;
using Cel.GameOfLife.Application.Models;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.CreateBoard;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.ResetBoard;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Queries.GetNextState;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Cel.GameOfLife.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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
        string boardId = await _mediator.Send(new CreateBoardCommand(board));

        return Ok(new ApiResponse<string>(boardId));
    }

    /// <summary>
    /// It generates and returns a next state for the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <returns>Next state.</returns>
    [HttpPut("{id}/next")]
    [ProducesResponseType(typeof(ApiResponse<List<List<bool>>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Put(string id)
    {
        List<List<bool>> nextState = await _mediator.Send(new GenerateNextStateCommand(id));

        return Ok(new ApiResponse<List<List<bool>>>(nextState));
    }

    /// <summary>
    /// It generates and returns a next state for the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <param name="count">Count to generete next states.</param>
    /// <returns>Next state.</returns>
    [HttpPut("{id}/next/{count}")]
    [ProducesResponseType(typeof(ApiResponse<List<List<bool>>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Put(string id, int count)
    {
        List<List<bool>> nextState = await _mediator.Send(new GenerateNextStateCommand(id, count));

        return Ok(new ApiResponse<List<List<bool>>>(nextState));
    }

    /// <summary>
    /// It reset to initial state.
    /// </summary>
    /// <param name="id">Id of board</param>
    [HttpPut("{id}/reset")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Reset(string id)
    {
        await _mediator.Send(new ResetBoardCommand(id));

        return Ok(new ApiResponse<bool>(true));
    }

    /// <summary>
    /// It returns the last state of the board.
    /// </summary>
    /// <param name="id">Id of board</param>
    /// <returns>last state of board</returns>
    [HttpGet("{id}/finalState")]
    [ProducesResponseType(typeof(ApiResponse<List<List<bool>>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetFinalState(string id)
    {
        List<List<bool>> nextState = await _mediator.Send(new GetFinalStateQuery(id));

        return Ok(new ApiResponse<List<List<bool>>>(nextState));
    }
}
