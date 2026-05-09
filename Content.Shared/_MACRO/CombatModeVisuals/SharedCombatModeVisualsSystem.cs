using Robust.Shared.Serialization;

namespace Content.Shared._MACRO.CombatModeVisuals;

public abstract class SharedCombatModeVisualsSystem : EntitySystem
{
    [Serializable, NetSerializable]
    public enum CombatModeVisualsVisuals : byte
    {
        Combat
    }
}
