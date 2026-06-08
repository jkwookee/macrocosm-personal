using Robust.Shared.Serialization;

namespace Content.Shared._MACRO.CombatMode;

public abstract partial class SharedCombatModeVisualsSystem : EntitySystem
{
    [Serializable, NetSerializable]
    public enum CombatModeVisualLayers : byte
    {
        Combat
    }
}
