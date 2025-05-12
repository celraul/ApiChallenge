using Microsoft.ML.Data;

namespace IA.Cel.TrainingModel.App.Inputs;

public class GameStateInput
{
    [LoadColumn(0)]
    public string PatternName { get; set; } = string.Empty;
    
    [LoadColumn(1)]
    public string Pattern { get; set; } 
}
