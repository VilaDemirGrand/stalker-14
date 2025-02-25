using Robust.Shared.Audio;

namespace Content.Shared._Stalker.Boombox;
[RegisterComponent]
public sealed partial class CassetteComponent : Component
{
    [DataField("sound")]
    public SoundSpecifier Music;

    public float Volume;
}
