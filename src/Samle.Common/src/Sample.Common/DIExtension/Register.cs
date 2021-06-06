using System;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Sample.Common.Service.Entity;
using Sample.Common.Service.Repositories;
using Sample.Common.Service.Settings;

namespace Sample.Common.Service.DI
{
    public static class Register
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDb = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDb.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });
            return services;
        }
        public static IServiceCollection AddRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddScoped<IRepository<T>>(serviceProvider =>
            {
                var mongoServiice = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(mongoServiice, collectionName);
            });
            return services;
        }
        public static IServiceCollection AddMassTransitAndRabbitMQ(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {
            services.AddMassTransit(x =>
       {
           x.AddConsumers(Assembly.GetEntryAssembly());
           x.UsingRabbitMq((context, configrator) =>
           {
               var rabittMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
               configrator.Host(rabittMQSettings.Host);
               configrator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceName, false));
               configrator.UseMessageRetry(retryConfigrator =>
               {
                   retryConfigrator.Interval(3, TimeSpan.FromMilliseconds(5));
               });
           });
       });
            services.AddMassTransitHostedService();
            return services;
        }
    }

}
