// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nuke;

/// <summary>
///     This generates a label for a nuclear bomb.
/// </summary>
/// <remarks>
///     This is a separate component because the fake nuclear bomb keg exists.
/// </remarks>
[RegisterComponent]
public sealed partial class NukeLabelComponent : Component
{
    [DataField] public LocId Prefix = "nuke-label-nanotrasen";
    [DataField] public int SerialLength = 6;
}
