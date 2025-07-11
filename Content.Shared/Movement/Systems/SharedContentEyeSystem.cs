// SPDX-FileCopyrightText: 2023 Artjom <artjombebenin@gmail.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.Administration;
using Content.Shared.Administration.Managers;
using Content.Shared.CCVar;
using Content.Shared.Ghost;
using Content.Shared.Input;
using Content.Shared.Movement.Components;
using Robust.Shared.Configuration;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;
using Robust.Shared.Serialization;

namespace Content.Shared.Movement.Systems;

/// <summary>
/// Lets specific sessions scroll and set their zoom directly.
/// </summary>
public abstract class SharedContentEyeSystem : EntitySystem
{
    [Dependency] private readonly ISharedAdminManager _admin = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;

    // Will be overridden according to config.
    public readonly Vector2 DefaultZoom = Vector2.One;
    public float ZoomMod { get; private set; } = 1f;
    public int ZoomLevels { get; private set; } = 1;
    public Vector2 MinZoom { get; private set; } = Vector2.One;

    [Dependency] private readonly SharedEyeSystem _eye = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ContentEyeComponent, ComponentStartup>(OnContentEyeStartup);
        SubscribeAllEvent<RequestTargetZoomEvent>(OnContentZoomRequest);
        SubscribeAllEvent<RequestEyeEvent>(OnRequestEye);

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.ZoomIn, InputCmdHandler.FromDelegate(ZoomIn, handle:false))
            .Bind(ContentKeyFunctions.ZoomOut, InputCmdHandler.FromDelegate(ZoomOut, handle:false))
            .Bind(ContentKeyFunctions.ResetZoom, InputCmdHandler.FromDelegate(ResetZoom, handle:false))
            .Register<SharedContentEyeSystem>();

        Log.Level = LogLevel.Info;
        UpdatesOutsidePrediction = true;

        Subs.CVar(_config, CCVars.ZoomLevelStep, value =>
        {
            ZoomMod = value;
            RecalculateZoomLevels();
        }, true);
        Subs.CVar(_config, CCVars.ZoomLevels, value =>
        {
            ZoomLevels = value;
            RecalculateZoomLevels();
        }, true);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        CommandBinds.Unregister<SharedContentEyeSystem>();
    }

    private void RecalculateZoomLevels()
    {
        MinZoom = DefaultZoom * (float) Math.Pow(ZoomMod, -ZoomLevels);
    }

    private void ResetZoom(ICommonSession? session)
    {
        if (TryComp(session?.AttachedEntity, out ContentEyeComponent? eye))
            ResetZoom(session.AttachedEntity.Value, eye);
    }

    private void ZoomOut(ICommonSession? session)
    {
        if (TryComp(session?.AttachedEntity, out ContentEyeComponent? eye))
            SetZoom(session.AttachedEntity.Value, eye.TargetZoom * ZoomMod, eye: eye);
    }

    private void ZoomIn(ICommonSession? session)
    {
        if (TryComp(session?.AttachedEntity, out ContentEyeComponent? eye))
            SetZoom(session.AttachedEntity.Value, eye.TargetZoom / ZoomMod, eye: eye);
    }

    private Vector2 Clamp(Vector2 zoom, ContentEyeComponent component)
    {
        return Vector2.Clamp(zoom, MinZoom, component.MaxZoom);
    }

    /// <summary>
    /// Sets the target zoom, optionally ignoring normal zoom limits.
    /// </summary>
    public void SetZoom(EntityUid uid, Vector2 zoom, bool ignoreLimits = false, ContentEyeComponent? eye = null)
    {
        if (!Resolve(uid, ref eye, false))
            return;

        eye.TargetZoom = ignoreLimits ? zoom : Clamp(zoom, eye);
        Dirty(uid, eye);
    }

    private void OnContentZoomRequest(RequestTargetZoomEvent msg, EntitySessionEventArgs args)
    {
        var ignoreLimit = msg.IgnoreLimit && _admin.HasAdminFlag(args.SenderSession, AdminFlags.Debug);

        if (TryComp<ContentEyeComponent>(args.SenderSession.AttachedEntity, out var content))
            SetZoom(args.SenderSession.AttachedEntity.Value, msg.TargetZoom, ignoreLimit, eye: content);
    }

    private void OnRequestEye(RequestEyeEvent msg, EntitySessionEventArgs args)
    {
        if (args.SenderSession.AttachedEntity is not { } player)
            return;

        if (!HasComp<GhostComponent>(player) && !_admin.IsAdmin(player))
            return;

        if (TryComp<EyeComponent>(player, out var eyeComp))
        {
            _eye.SetDrawFov(player, msg.DrawFov, eyeComp);
            _eye.SetDrawLight((player, eyeComp), msg.DrawLight);
        }
    }

    private void OnContentEyeStartup(EntityUid uid, ContentEyeComponent component, ComponentStartup args)
    {
        if (!TryComp<EyeComponent>(uid, out var eyeComp))
            return;

        _eye.SetZoom(uid, component.TargetZoom, eyeComp);
        Dirty(uid, component);
    }

    public void ResetZoom(EntityUid uid, ContentEyeComponent? component = null)
    {
        SetZoom(uid, DefaultZoom, eye: component);
    }

    public void SetMaxZoom(EntityUid uid, Vector2 value, ContentEyeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.MaxZoom = value;
        component.TargetZoom = Clamp(component.TargetZoom, component);
        Dirty(uid, component);
    }

    /// <summary>
    /// Sendable from client to server to request a target zoom.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class RequestTargetZoomEvent : EntityEventArgs
    {
        public Vector2 TargetZoom;
        public bool IgnoreLimit;
    }

    /// <summary>
    /// Sendable from client to server to request changing fov.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class RequestEyeEvent : EntityEventArgs
    {
        public readonly bool DrawFov;
        public readonly bool DrawLight;

        public RequestEyeEvent(bool drawFov, bool drawLight)
        {
            DrawFov = drawFov;
            DrawLight = drawLight;
        }
    }
}
