using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UuidKeySharp.Interfaces;

namespace UuidKeySharp.Test.Setup;

public class TestFixture : IDisposable
{    public IServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IUuidKey, UuidKey>();
            })
            .Build();
 
        ServiceProvider = host.Services;
    }
 
    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}