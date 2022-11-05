using ConsoleEngine.Services.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEngine.Services.Factories
{
    public interface IEntityRepositoryFactory : IService
    {
        Dictionary<Type, IEntityRepository> CreateRepositories();
    }
}
