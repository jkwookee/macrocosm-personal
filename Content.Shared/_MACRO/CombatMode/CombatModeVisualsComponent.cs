using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared._MACRO.CombatMode;

/// <summary>
/// Allows the use of unique sprites for combat mode
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CombatModeVisualsComponent : Component
{
    /// <summary>
    /// Hides the base layer when combat mode is activated if it exists
    /// </summary>
    [DataField]
    public bool HideBaseLayer;
}
