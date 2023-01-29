using System.Reflection;
namespace fika_yazılım_test.Services;

public class AppVersionService : IAppVersionService
{
    public string Version => Assembly.GetEntryAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        .InformationalVersion;
}

