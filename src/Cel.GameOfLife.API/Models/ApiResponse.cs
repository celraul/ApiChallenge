namespace Cel.GameOfLife.API.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public List<string> errors { get; set; } = new();

    public ApiResponse() { }

    public ApiResponse(T data, bool success = true)
    {
        Data = data;
        Success = success;
    }
}