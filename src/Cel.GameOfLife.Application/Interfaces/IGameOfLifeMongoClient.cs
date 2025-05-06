using MongoDB.Driver;

namespace Cel.GameOfLife.Application.Interfaces;

public interface IGameOfLifeMongoClient
{
    IMongoCollection<T> GetCollection<T>();
}