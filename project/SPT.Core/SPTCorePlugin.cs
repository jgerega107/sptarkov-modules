using System;
using BepInEx;
using BepInEx.Configuration;
using SPT.Common;
using SPT.Core.Patches;

namespace SPT.Core;

[BepInPlugin("com.SPT.core", "SPT.Core", SPTPluginInfo.PLUGIN_VERSION)]
public class SPTCorePlugin : BaseUnityPlugin
{
    // Temp static logger field, remove along with plugin whitelisting before release
    internal static BepInEx.Logging.ManualLogSource _logger;

    internal static ConfigEntry<bool> DisableSslValidation { get; private set; }

    public void Awake()
    {
        _logger = Logger;

        DisableSslValidation = Config.Bind(
            "Security",
            "DisableSslValidation",
            false,
            "When enabled, SSL certificate validation is bypassed for all connections. Only enable this if your SPT server uses self-signed certificates.");

        Logger.LogInfo("Loading: SPT.Core");

        try
        {
            new ConsistencySinglePatch().Enable();
            new ConsistencyMultiPatch().Enable();
            new GameValidationPatch().Enable();
            new BattlEyePatch().Enable();
            new UnityWebRequestPatch().Enable();
            new UnityWebRequestAuthPatch().Enable();
            new Patch4001().Enable();
            new Patch4002().Enable();
            new SslCertificatePatch().Enable();
            new WebSocketSslValidationPatch().Enable();
        }
        catch (Exception ex)
        {
            Logger.LogError($"A PATCH IN {GetType().Name} FAILED. SUBSEQUENT PATCHES HAVE NOT LOADED");
            Logger.LogError($"{GetType().Name}: {ex}");

            throw;
        }

        Logger.LogInfo("Completed: SPT.Core");
    }
}
