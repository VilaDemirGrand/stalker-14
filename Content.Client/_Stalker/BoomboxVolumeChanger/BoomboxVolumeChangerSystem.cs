using Content.Shared._Stalker.Boombox;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Client._Stalker.BoomboxVolumeChanger;

public sealed class BoomboxVolumeChangerSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;

    public override void Initialize()
    {
        base .Initialize();
        _configurationManager.OnValueChanged(CCVars.BoomboxVolume, SetBoomboxVolume, true);
    }

    private void SetBoomboxVolume(float volume)
    {
        var query = EntityQueryEnumerator<BoomboxComponent>();
        while (query.MoveNext(out var ent, out var comp))
        {
            comp.Volume = volume;
        }
    }
}
