// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Shitmed.GoliathTentacle;

/// <summary>
/// Component that grants the entity the ability to use goliath tentacles.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class GoliathTentacleComponent : Component
{
    //[DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    //public string? Action = "ActionGoliathTentacleCrew";

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;
}
