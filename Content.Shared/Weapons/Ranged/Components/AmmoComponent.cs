using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.Damage;
using Content.Shared.Physics;
using Content.Shared.Weapons.Reflect;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
/// Allows the entity to be fired from a gun.
/// </summary>
[RegisterComponent, Virtual]
public partial class AmmoComponent : Component, IShootable
{
    // Muzzle flash stored on ammo because if we swap a gun to whatever we may want to override it.

    [ViewVariables(VVAccess.ReadWrite), DataField("muzzleFlash", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string? MuzzleFlash = "MuzzleFlashEffect";
}

/// <summary>
/// Spawns another prototype to be shot instead of itself.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CartridgeAmmoComponent : AmmoComponent
{
    [ViewVariables(VVAccess.ReadWrite), DataField("proto", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string Prototype = default!;

    [ViewVariables(VVAccess.ReadWrite), DataField("spent")]
    [AutoNetworkedField]
    public bool Spent = false;

    // stalker-changes-start

    [ViewVariables(VVAccess.ReadWrite), DataField("hitscan")]
    [AutoNetworkedField]
    public bool Hitscan = false;

    [DataField("projectileClass", false, 1, false, false, null)]
    public int? ProjectileClass;


    // this shit is weird idk how to do it another way

    [ViewVariables(VVAccess.ReadWrite), DataField("staminaDamage")]
    public float StaminaDamage;

    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public DamageSpecifier? Damage;

    [DataField("collisionMask")]
    public int CollisionMask = (int)CollisionGroup.Opaque;

    [DataField("reflective")] public ReflectType Reflective = ReflectType.NonEnergy;

    [DataField("sound")]
    public SoundSpecifier? Sound;

    [ViewVariables(VVAccess.ReadWrite), DataField("forceSound")]
    public bool ForceSound;

    [DataField("maxLength")]
    public float MaxLength = 45f;

    // stalker-changes-end

    /// <summary>
    /// Caseless ammunition.
    /// </summary>
    [DataField("deleteOnSpawn")]
    public bool DeleteOnSpawn;

    [DataField("soundEject")]
    public SoundSpecifier? EjectSound = new SoundCollectionSpecifier("CasingEject");
}
