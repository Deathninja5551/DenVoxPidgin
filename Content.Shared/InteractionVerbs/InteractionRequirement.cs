// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Serialization;

namespace Content.Shared.InteractionVerbs;

/// <summary>
///     Defines a requirement for an <see cref="InteractionVerb"/>.
///     If a verb does not meet the requirement, it will be hidden or disabled in the verb menu.
/// </summary>
[ImplicitDataDefinitionForInheritors, Serializable, NetSerializable]
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors )]
public abstract partial class InteractionRequirement
{
    public abstract bool IsMet(InteractionArgs args, InteractionVerbPrototype proto, InteractionAction.VerbDependencies deps);
}

/// <inheritdoc cref="InteractionRequirement"/>
[Serializable, NetSerializable]
public abstract partial class InvertableInteractionRequirement : InteractionRequirement
{
    [DataField] public bool Inverted = false;
}
