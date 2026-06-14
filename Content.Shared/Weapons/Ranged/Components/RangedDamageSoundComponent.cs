using Content.Shared.Damage.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
/// Plays the specified sound upon receiving damage of that type.
/// </summary>
[RegisterComponent]
public sealed partial class RangedDamageSoundComponent : Component
{
    [DataField("soundGroups",
        customTypeSerializer: typeof(PrototypeIdDictionarySerializer<SoundSpecifier, DamageGroupPrototype>))]
    public Dictionary<string, SoundSpecifier>? SoundGroups;

    [DataField("soundTypes",
         customTypeSerializer: typeof(PrototypeIdDictionarySerializer<SoundSpecifier, DamageTypePrototype>))]
    public Dictionary<string, SoundSpecifier>? SoundTypes;
}
