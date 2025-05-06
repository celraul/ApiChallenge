using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Cel.GameOfLife.Infra.Repositories;

public class Repository<T>(IGameOfLifeMongoClient mongoClient) : IRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection = mongoClient.GetCollection<T>();

    public async Task<T> GetById(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<T> InsertAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
        await _collection.ReplaceOneAsync(filter, entity);
    }
}