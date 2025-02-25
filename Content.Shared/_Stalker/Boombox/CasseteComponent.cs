using Robust.Shared.Audio;

namespace Content.Shared._Stalker.Boombox;
[RegisterComponent]
public sealed partial class CasseteComponent : Component
{
    [DataField("sound")]
    public SoundSpecifier Music;
}
