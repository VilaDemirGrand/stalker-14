using Robust.Shared.Audio.Components;

namespace Content.Shared._Stalker.Boombox;

[RegisterComponent]
public sealed partial class BoomboxComponent : Component
{

    [DataField]
    public float Volume = -7f;

    [DataField]
    public float MaxDistance = 7f;

    [DataField]
    public (EntityUid, AudioComponent)? CurrentPlaying;

    [DataField]
    public TimeSpan SoundTime = TimeSpan.Zero;

    [DataField]
    public bool RepeatOn = false;
    public TimeSpan SoundEnd = TimeSpan.Zero;
}
