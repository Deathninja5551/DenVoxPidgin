// SPDX-FileCopyrightText: 2020 Vince <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2020 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2022 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2022 ScalyChimp <72841710+scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Winkarst <74284083+Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Administration.Logs;
using Content.Server.Atmos.Components;
using Content.Server.Body.Systems;
using Content.Server.Fluids.EntitySystems;
using Content.Server.NodeContainer.EntitySystems;
using Content.Shared.Atmos.EntitySystems;
using Content.Shared.Decals;
using Content.Shared.Doors.Components;
using Content.Shared.Maps;
using Content.Shared.Standing;
using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Atmos.EntitySystems;

/// <summary>
///     This is our SSAir equivalent, if you need to interact with or query atmos in any way, go through this.
/// </summary>
[UsedImplicitly]
public sealed partial class AtmosphereSystem : SharedAtmosphereSystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDefinitionManager = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly InternalsSystem _internals = default!;
    [Dependency] private readonly SharedContainerSystem _containers = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly GasTileOverlaySystem _gasTileOverlaySystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;
    [Dependency] private readonly TileSystem _tile = default!;
    [Dependency] private readonly MapSystem _map = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] public readonly PuddleSystem Puddle = default!;
    [Dependency] private readonly ThrowingSystem _throwing = default!;
    [Dependency] private readonly ThrownItemSystem _thrown = default!;
    [Dependency] private readonly SharedStunSystem _sharedStunSystem = default!;
    [Dependency] private readonly StandingStateSystem _standingSystem = default!;

    private const float ExposedUpdateDelay = 1f;
    private float _exposedTimer = 0f;

    private EntityQuery<GridAtmosphereComponent> _atmosQuery;
    private EntityQuery<MapAtmosphereComponent> _mapAtmosQuery;
    private EntityQuery<AirtightComponent> _airtightQuery;
    private EntityQuery<FirelockComponent> _firelockQuery;
    private HashSet<EntityUid> _entSet = new();

    private string[] _burntDecals = [];

    public override void Initialize()
    {
        base.Initialize();

        UpdatesAfter.Add(typeof(NodeGroupSystem));

        InitializeBreathTool();
        InitializeGases();
        InitializeCommands();
        InitializeCVars();
        InitializeGridAtmosphere();
        InitializeMap();

        _mapAtmosQuery = GetEntityQuery<MapAtmosphereComponent>();
        _atmosQuery = GetEntityQuery<GridAtmosphereComponent>();
        _airtightQuery = GetEntityQuery<AirtightComponent>();
        _firelockQuery = GetEntityQuery<FirelockComponent>();

        SubscribeLocalEvent<TileChangedEvent>(OnTileChanged);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(OnPrototypesReloaded);

        CacheDecals();
    }

    public override void Shutdown()
    {
        base.Shutdown();

        ShutdownCommands();
    }

    private void OnTileChanged(ref TileChangedEvent ev)
    {
        foreach (var change in ev.Changes)
        {
            InvalidateTile(ev.Entity.Owner, change.GridIndices);
        }
    }

    private void OnPrototypesReloaded(PrototypesReloadedEventArgs ev)
    {
        if (ev.WasModified<DecalPrototype>())
            CacheDecals();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        UpdateProcessing(frameTime);
        UpdateHighPressure(frameTime);

        _exposedTimer += frameTime;

        if (_exposedTimer < ExposedUpdateDelay)
            return;

        var query = EntityQueryEnumerator<AtmosExposedComponent>();
        while (query.MoveNext(out var uid, out _))
        {
            var transform = Transform(uid);
            var air = GetContainingMixture((uid, transform));

            if (air == null)
                continue;

            var updateEvent = new AtmosExposedUpdateEvent(transform.Coordinates, air, transform);
            RaiseLocalEvent(uid, ref updateEvent);
        }

        _exposedTimer -= ExposedUpdateDelay;
    }

    private void CacheDecals()
    {
        _burntDecals = _prototypeManager.EnumeratePrototypes<DecalPrototype>().Where(x => x.Tags.Contains("burnt")).Select(x => x.ID).ToArray();
    }
}
