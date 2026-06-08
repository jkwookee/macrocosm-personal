using Content.Client.DamageState;
using Content.Shared._MACRO.CombatMode;
using Content.Shared.CombatMode;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Robust.Client.GameObjects;

namespace Content.Client._MACRO.CombatMode;

public sealed partial class CombatModeVisualsSystem : SharedCombatModeVisualsSystem
{
    [Dependency] private AppearanceSystem _appearance = default!;
    [Dependency] private MobStateSystem _mobState = default!;
    [Dependency] private SpriteSystem _sprite = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CombatModeVisualsComponent, ToggleCombatActionEvent>(
            OnCombatToggle,
            after: [typeof(SharedCombatModeSystem)]);
        SubscribeLocalEvent<CombatModeVisualsComponent, MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<CombatModeVisualsComponent, AppearanceChangeEvent>(OnAppearanceChanged);
    }

    private void OnCombatToggle(Entity<CombatModeVisualsComponent> ent, ref ToggleCombatActionEvent args)
    {
        ChangeAppearance(ent);
    }

    private void OnMobStateChanged(Entity<CombatModeVisualsComponent> ent, ref MobStateChangedEvent args)
    {
        ChangeAppearance(ent);
    }

    private void ChangeAppearance(EntityUid ent)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        _appearance.OnChangeData(ent, sprite);
    }

    private void OnAppearanceChanged(Entity<CombatModeVisualsComponent> ent, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;
        if (!TryComp<CombatModeComponent>(ent, out var combat))
            return;

        // make sure we can sync the frames
        if (!_sprite.TryGetLayer((ent, args.Sprite), CombatModeVisualLayers.Combat, out var combatLayer, true))
            return;

        // turn on combat visuals if the mob is alive and in combat mode. otherwise turn them off
        _sprite.LayerSetVisible(combatLayer, _mobState.IsAlive(ent) && combat.IsInCombatMode);

        if (!_sprite.TryGetLayer((ent, args.Sprite), DamageStateVisualLayers.Base, out var baseLayer, true))
            return;

        // handle hiding/unhiding the base layer if applicable
        if (ent.Comp.HideBaseLayer && _mobState.IsAlive(ent))
            _sprite.LayerSetVisible(baseLayer, !combat.IsInCombatMode);
        else if (ent.Comp.HideBaseLayer)
            _sprite.LayerSetVisible(baseLayer, true);

        // then sync them to the base animation
        if (combatLayer.AutoAnimated)
            _sprite.LayerSetAnimationTime(combatLayer, baseLayer.AnimationTime);
    }
}
