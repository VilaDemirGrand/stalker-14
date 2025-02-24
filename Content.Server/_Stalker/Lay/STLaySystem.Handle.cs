using Content.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;
using Content.Shared._Stalker.Lay;

namespace Content.Server._Stalker.Lay;

public sealed partial class STLaySystem
{
    private void InitializeHandle()
    {
        CommandBinds.Builder
            .Bind(ContentKeyFunctions.Lay, InputCmdHandler.FromDelegate(HandleLay, handle: false, outsidePrediction: false))
            .Register<STLaySystem>();
    }

    private void HandleLay(ICommonSession? session)
    {
        var entity = session?.AttachedEntity;
        if (entity is null)
            return;

        if (!TryComp<STLayComponent>(entity, out var comp))
            return;

        var nextState = comp.StateTransitions[comp.State];
        StartSetState((entity.Value, comp), nextState);
    }
}
