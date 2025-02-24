using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.GunRequiresLay;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(GunRequiresLaySystem))]
public sealed partial class GunRequiresLayComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan LastPopup;

    [DataField, AutoNetworkedField]
    public TimeSpan PopupCooldown = TimeSpan.FromSeconds(1);
}
