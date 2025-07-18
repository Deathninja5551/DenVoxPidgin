// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Administration;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Server.Player;

namespace Content.Server.Administration.Commands;

/// <summary>
/// Opens the job whitelists panel for editing player whitelists.
/// To use this ingame it's easiest to first open the player panel, then hit Job Whitelists.
/// </summary>
[AdminCommand(AdminFlags.Whitelist)]
public sealed class JobWhitelistsCommand : LocalizedCommands
{
    [Dependency] private readonly EuiManager _eui = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    public override string Command => "jobwhitelists";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not {} player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        if (args.Length != 1)
        {
            shell.WriteLine(Loc.GetString("cmd-ban-invalid-arguments"));
            shell.WriteLine(Help);
        }

        var playerarg = string.Join(' ', args).Trim();
        var located = await _locator.LookupIdByNameAsync(playerarg);
        if (located is null)
        {
            shell.WriteError(Loc.GetString("cmd-jobwhitelists-player-err"));
            return;
        }

        var ui = new JobWhitelistsEui(located.UserId, located.Username);
        ui.LoadWhitelists();
        _eui.OpenEui(ui, player);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                _players.Sessions.Select(s => s.Name),
                Loc.GetString("cmd-jobwhitelist-hint-player"));
        }

        return CompletionResult.Empty;
    }
}
