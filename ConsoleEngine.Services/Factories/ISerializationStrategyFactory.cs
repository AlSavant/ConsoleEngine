using ConsoleEngine.Services.AssetManagement.Strategies;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Factories
{
    internal interface ISerializationStrategyFactory : IService
    {
        Dictionary<string, ISerializationStrategy> CreateInstances();
    }
}
