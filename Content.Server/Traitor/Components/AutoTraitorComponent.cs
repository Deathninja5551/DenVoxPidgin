// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Errant <35878406+Errant-4@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Traitor.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server.Traitor.Components;

/// <summary>
/// Makes the entity a traitor either instantly if it has a mind or when a mind is added.
/// </summary>
[RegisterComponent, Access(typeof(AutoTraitorSystem))]
public sealed partial class AutoTraitorComponent : Component
{
    /// <summary>
    /// The traitor profile to use
    /// </summary>
    [DataField]
    public EntProtoId Profile = "Traitor";
}
