using Content.Shared._Misfits.Special;
using Content.Shared._Misfits.Special.Components;
using Content.Shared.Damage.Components;

namespace Content.Shared._Misfits.SpecialStats;

/// <summary>
/// Applies Endurance body resilience bonuses when S.P.E.C.I.A.L. changes.
/// </summary>
public sealed class SpecialEnduranceSystem : EntitySystem
{
    [Dependency] private readonly SharedSpecialSystem _special = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpecialComponent, SpecialChangedEvent>(OnSpecialChanged);
        SubscribeLocalEvent<SpecialComponent, SpecialStatsReadyEvent>(OnStatsReady);
    }

    private void OnSpecialChanged(Entity<SpecialComponent> ent, ref SpecialChangedEvent args)
    {
        ApplyEndurance(ent);
    }

    private void OnStatsReady(Entity<SpecialComponent> ent, ref SpecialStatsReadyEvent args)
    {
        ApplyEndurance(ent);
    }

    private void ApplyEndurance(Entity<SpecialComponent> ent)
    {
        if (!TryComp<StaminaComponent>(ent.Owner, out var stamina))
            return;

        var tuning = _special.GetTuning();
        var delta = _special.GetCurvedEffectDelta(ent.Owner, SpecialStat.Endurance, ent.Comp);
        var desired = delta * tuning.EnduranceStaminaCritThresholdPerPoint;
        var adjustment = desired - ent.Comp.AppliedStaminaCritThresholdModifier;

        if (MathHelper.CloseTo(adjustment, 0f))
            return;

        stamina.CritThreshold += adjustment;
        ent.Comp.AppliedStaminaCritThresholdModifier = desired;

        Dirty(ent.Owner, stamina);
        Dirty(ent.Owner, ent.Comp);
    }

}
