namespace DependencyInjection
{
    public interface IServiceCollection
    {
        IServiceCollection AddOption(string key, object value);
        T GetOption<T>(string key);
        object GetOption(string key);
        IServiceProvider BuildProvider();
    }
}
