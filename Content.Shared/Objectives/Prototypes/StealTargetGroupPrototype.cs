// SPDX-FileCopyrightText: 2023 Ed <96445749+theshued@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Objectives;

/// <summary>
/// General data about a group of items, such as icon, description, name. Used for Steal objective
/// </summary>
[Prototype("stealTargetGroup")]
public sealed partial class StealTargetGroupPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;
    [DataField] public string Name { get; private set; } = string.Empty;
    [DataField] public SpriteSpecifier Sprite { get; private set; } = SpriteSpecifier.Invalid;
}
