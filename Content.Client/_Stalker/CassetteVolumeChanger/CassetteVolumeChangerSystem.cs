using Robust.Client.Audio;
using Content.Shared._Stalker.Boombox;

namespace Content.Client._Stalker.CassetteVolumeChanger;

public sealed class CassetteVolumeChangerSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audioSystem = default!;

    public override void Initialize()
    {
        base .Initialize();

    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<CassetteComponent>();
    }

    private void ChangeVolume(float volume)
    {

    }
}
