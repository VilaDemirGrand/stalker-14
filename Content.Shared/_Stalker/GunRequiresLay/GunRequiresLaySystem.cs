using Content.Shared.Examine;
using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared._Stalker.Lay;
using Robust.Shared.Timing;

namespace Content.Shared._Stalker.GunRequiresLay;
public sealed class GunRequiresLaySystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunRequiresLayComponent, ExaminedEvent>(OnExamineRequires);
        SubscribeLocalEvent<GunRequiresLayComponent, ShotAttemptedEvent>(OnShootAttempt);


    }

    private void OnShootAttempt(EntityUid uid, GunRequiresLayComponent component, ref ShotAttemptedEvent args)
    {
        if (TryComp<STLayComponent>(args.User, out var comp) && comp.State == STLayState.Stand)
        {

            args.Cancel();

            var time = _timing.CurTime;
            if (time > component.LastPopup + component.PopupCooldown)
            {
                component.LastPopup = time;
                var message = Loc.GetString("gun-requires-lay-cancel");
                _popupSystem.PopupClient(message, args.Used, args.User);
            }
        }
    }

    private void OnExamineRequires(Entity<GunRequiresLayComponent> entity, ref ExaminedEvent args)
    {
        args.PushText(Loc.GetString("gun-requires-lay-examine"));
    }
}
