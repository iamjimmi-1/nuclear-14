using Robust.Shared.Prototypes;

namespace Content.Shared._Misfits.Crafting;

[Prototype]
public sealed partial class HandCraftIntellRecipePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; set; } = default!;

    [DataField(required: true)]
    public int MinInt;
}
