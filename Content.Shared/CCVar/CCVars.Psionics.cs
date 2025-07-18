// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///    Whether glimmer is enabled.
    /// </summary>
    public static readonly CVarDef<bool> GlimmerEnabled =
        CVarDef.Create("glimmer.enabled", true, CVar.REPLICATED);

    /// <summary>
    ///     The rate at which glimmer linearly decays. Since glimmer increases (usually) follow a logistic curve, this means glimmer
    ///     becomes increasingly harder to raise after ~502 points.
    /// </summary>
    public static readonly CVarDef<float> GlimmerLinearDecayPerSecond =
        CVarDef.Create("glimmer.linear_decay_per_second", 1f, CVar.SERVERONLY);

    /// <summary>
    ///     How many seconds between updates to passive glimmer decay.
    /// </summary>
    public static readonly CVarDef<float> GlimmerDecayUpdateInterval =
        CVarDef.Create("glimmer.decay_update_interval", 10f, CVar.SERVERONLY);

    /// <summary>
    ///     Whether random rolls for psionics are allowed.
    ///     Guaranteed psionics will still go through.
    /// </summary>
    public static readonly CVarDef<bool> PsionicRollsEnabled =
        CVarDef.Create("psionics.rolls_enabled", true, CVar.SERVERONLY);

    /// <summary>
    ///     When mindbroken, permanently eject the player from their own body, and turn their character into an NPC.
    ///     Congratulations, now they *actually* aren't a person anymore.
    ///     For people who complained that it wasn't obvious enough from the text that Mindbreaking is a form of Murder.
    /// </summary>
    public static readonly CVarDef<bool> ScarierMindbreaking =
        CVarDef.Create("psionics.scarier_mindbreaking", false, CVar.SERVERONLY);

    /// <summary>
    /// Allow Ethereal Ent to PassThrough Walls/Objects while in Ethereal.
    /// </summary>
    public static readonly CVarDef<bool> EtherealPassThrough =
        CVarDef.Create("ic.EtherealPassThrough", false, CVar.SERVER);
}
