namespace SPT.Common.Http;

public class ServerConfig(
    string backendUrl,
    string matchingVersion,
    string version,
    string basicAuthUsername = "",
    string basicAuthPassword = ""
)
{
    public string BackendUrl { get; } = backendUrl;
    public string MatchingVersion { get; } = matchingVersion;
    public string Version { get; } = version;
    public string BasicAuthUsername { get; } = basicAuthUsername ?? "";
    public string BasicAuthPassword { get; } = basicAuthPassword ?? "";
}
