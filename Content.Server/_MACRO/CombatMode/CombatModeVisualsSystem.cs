using Content.Shared._MACRO.CombatMode;
using Content.Shared.CombatMode;
using Content.Shared.Mobs;

namespace Content.Server._MACRO.CombatMode;

public sealed partial class CombatModeVisualsSystem : SharedCombatModeVisualsSystem
{
    [Dependency] private SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CombatModeVisualsComponent, ToggleCombatActionEvent>(
            OnCombatToggle,
            after: [typeof(SharedCombatModeSystem)]);
        SubscribeLocalEvent<CombatModeVisualsComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnCombatToggle(Entity<CombatModeVisualsComponent> ent, ref ToggleCombatActionEvent args)
    {
        if (TryComp<CombatModeComponent>(ent, out var combat))
            _appearance.SetData(ent, CombatModeVisualsVisuals.Combat, combat.IsInCombatMode);
    }

    private void OnMobStateChanged(Entity<CombatModeVisualsComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Critical || args.NewMobState == MobState.Dead)
            _appearance.SetData(ent, CombatModeVisualsVisuals.Combat, false);
    }
}
