// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[RegisterComponent, NetworkedComponent]
[Prototype("SurgerySteps")]
public sealed partial class SurgeryStepComponent : Component
{

    [DataField]
    public ComponentRegistry? Tool;

    [DataField]
    public ComponentRegistry? Add;

    [DataField]
    public ComponentRegistry? BodyAdd;

    [DataField]
    public ComponentRegistry? Remove;

    [DataField]
    public ComponentRegistry? BodyRemove;

    /// <summary>
    ///   These components will be added to the body part's organs' OnAdd field.
    ///   Each key is the SlotId of the organ to look for.
    ///
    ///   Used to make organs add components to whatever body it's residing in.
    /// </summary>
    [DataField]
    public Dictionary<string, ComponentRegistry>? AddOrganOnAdd;

    /// <summary>
    ///   These components will be removed from the body part's organs' OnAdd field.
    ///   Each key is the SlotId of the organ to look for.
    ///
    ///   Used to stop organs from adding components to whatever body it's residing in.
    /// </summary>
    [DataField]
    public Dictionary<string, ComponentRegistry>? RemoveOrganOnAdd;

    [DataField]
    public float Duration = 2f;
}
