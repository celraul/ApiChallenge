using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;
using Cel.GameOfLife.Infra.Cache;
using Cel.GameOfLife.Infra.Mongo;
using Cel.GameOfLife.Infra.Options;
using Cel.GameOfLife.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Cel.GameOfLife.Infra;

public static class ConfigureServices
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongo(configuration)
            .AddRepositories()
            .AddCache();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)

    {
        services.Configure<MongoSettings>(option => configuration.GetSection(nameof(MongoSettings)).Bind(option));

        services.AddSingleton<IGameOfLifeMongoClient, GameOfLifeMongoClient>();

        BsonClassMap.RegisterClassMap<BaseEntity>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapIdProperty(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });

        var ingnoreExtraElements = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("Ignore extra elements", ingnoreExtraElements, t => true);

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IAppMemoryCache, AppMemoryCache>();

        return services;
    }
}