// See https://aka.ms/new-console-template for more information
using IA.Cel.TrainingModel.App.Inputs;
using Microsoft.ML;

Console.WriteLine("Hello, World!");


var context = new MLContext();

IDataView data = context.Data.LoadFromTextFile<GameStateInput>("data.csv", separatorChar: ',', hasHeader: true);

var pipeline = context.Transforms.Conversion.MapValueToKey("PatternName")
    .Append(context.Transforms.NormalizeMinMax("Pattern"))
    .Append(context.MulticlassClassification.Trainers.SdcaMaximumEntropy())
    .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));


var model = pipeline.Fit(data);
var predictions = model.Transform(data);
var metrics = context.MulticlassClassification.Evaluate(predictions);

Console.WriteLine($"Accuracy: {metrics.MacroAccuracy}");

context.Model.Save(model, data.Schema, "GameOfLifeModel.zip");

Console.WriteLine("end.");