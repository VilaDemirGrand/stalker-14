using Content.Shared.Examine;
using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Events;
using Content.Server._Stalker.Lay;

namespace Content.Server._Stalker.GunRequiresLay;
public sealed class GunRequiresLaySystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunRequiresLayComponent, ExaminedEvent>(OnExamineRequires);
        SubscribeLocalEvent<GunRequiresLayComponent, ShotAttemptedEvent>(OnShootAttempt);


    }

    private void OnShootAttempt(EntityUid uid, GunRequiresLayComponent component, ref ShotAttemptedEvent args)
    {
        if (TryComp<STLayComponent>(args.User, out var comp) && comp.State == Shared._Stalker.Lay.STLayState.Stand)
        {

            args.Cancel();
            var message = Loc.GetString("gun-requires-lay-cancel");
            _popupSystem.PopupClient(message, args.Used, args.User);
        }
    }

    private void OnExamineRequires(Entity<GunRequiresLayComponent> entity, ref ExaminedEvent args)
    {
        args.PushText(Loc.GetString("gun-requires-lay-examine"));
    }
}
