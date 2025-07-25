// SPDX-FileCopyrightText: 2022 Kara D <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Drunk;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client.Drunk;

public sealed class DrunkSystem : SharedDrunkSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private DrunkOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DrunkComponent, ComponentInit>(OnDrunkInit);
        SubscribeLocalEvent<DrunkComponent, ComponentShutdown>(OnDrunkShutdown);

        SubscribeLocalEvent<DrunkComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<DrunkComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, DrunkComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, DrunkComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.CurrentBoozePower = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnDrunkInit(EntityUid uid, DrunkComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnDrunkShutdown(EntityUid uid, DrunkComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.CurrentBoozePower = 0;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}
