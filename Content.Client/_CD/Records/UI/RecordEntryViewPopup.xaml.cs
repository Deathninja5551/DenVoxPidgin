// SPDX-FileCopyrightText: 2025 Lyndomen <49795619+Lyndomen@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.UserInterface.Controls;
using Content.Shared._CD.Records;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.RichText;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client._CD.Records.UI;

[GenerateTypedNameReferences]
public sealed partial class RecordEntryViewPopup : FancyWindow
{
    // Font tags bad
    private static readonly Type[] AllowedTags =
    [
        typeof(BoldItalicTag),
        typeof(BoldTag),
        typeof(BulletTag),
        typeof(ColorTag),
        typeof(HeadingTag),
        typeof(ItalicTag),
    ];

    public RecordEntryViewPopup()
    {
        RobustXamlLoader.Load(this);

        // Create a styled box for the content panel
        var styleBox = new StyleBoxFlat
        {
            BackgroundColor = CharacterRecordViewer.ContentPanelColor,
            BorderColor = CharacterRecordViewer.BorderColor,
            BorderThickness = new Thickness(1),
        };

        ContentPanel.PanelOverride = styleBox;
    }

    public void SetContents(PlayerProvidedCharacterRecords.RecordEntry entry)
    {
        EntryTitle.SetMessage(entry.Title);
        EntryInvolved.SetMessage(entry.Involved);
        EntryDesc.SetMessage(FormattedMessage.FromMarkupPermissive(entry.Description.Trim()), AllowedTags);
    }
}
