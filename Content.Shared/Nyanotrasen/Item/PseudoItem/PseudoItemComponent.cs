// SPDX-FileCopyrightText: 2023 Alzore <140123969+blackern5000@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Item;
using Robust.Shared.Prototypes;

namespace Content.Shared.Nyanotrasen.Item.PseudoItem;

/// <summary>
/// For entities that behave like an item under certain conditions,
/// but not under most conditions.
/// </summary>
[RegisterComponent, AutoGenerateComponentState]
public sealed partial class PseudoItemComponent : Component
{
    [DataField("size")]
    public ProtoId<ItemSizePrototype> Size = "Huge";

    /// <summary>
    /// An optional override for the shape of the item within the grid storage.
    /// If null, a default shape will be used based on <see cref="Size"/>.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<Box2i>? Shape;

    [DataField, AutoNetworkedField]
    public Vector2i StoredOffset;

    public bool Active = false;

    /// <summary>
    /// Action for sleeping while inside a container with <see cref="AllowsSleepInsideComponent"/>.
    /// </summary>
    [DataField]
    public EntityUid? SleepAction;
}
