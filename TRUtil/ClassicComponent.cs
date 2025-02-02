﻿using LiveSplit.Model;                    // LiveSplitState
using LiveSplit.UI;                       // LayoutMode
using LiveSplit.UI.Components;            // IComponent, LogicComponent, SettingsHelper
using LiveSplit.UI.Components.AutoSplit;  // AutoSplitComponent, IAutoSplitter
using System;                             // EventArgs, IDisposable
using System.Windows.Forms;               // Control, TableLayoutPanel
using System.Xml;                         // XmlDocument, XmlElement, XmlNode

namespace TRUtil;

/// <summary>
///     Implementation of <see cref="AutoSplitComponent"/>.
/// </summary>
/// <remarks>
///     <see cref="AutoSplitComponent"/> is derived from <see cref="LogicComponent"/>,
///     which derives from <see cref="IComponent"/> and <see cref="IDisposable"/>.
/// </remarks>
public abstract class ClassicComponent : AutoSplitComponent
{
    private readonly ClassicAutosplitter _splitter;
    private readonly LiveSplitState _state;

    private void StateOnStart(object _0, EventArgs _1) => _splitter?.OnStart();
    private void StateOnSplit(object _0, EventArgs _1) => _splitter?.OnSplit(BaseGameData.Level.Current);
    private void StateOnUndoSplit(object _0, EventArgs _1) => _splitter?.OnUndoSplit();

    /// <inheritdoc/>
    protected ClassicComponent(ClassicAutosplitter autosplitter, LiveSplitState state) : base(autosplitter, state)
    {
        _splitter = autosplitter;
        _state = state;
        _state.OnSplit += StateOnSplit;
        _state.OnStart += StateOnStart;
        _state.OnUndoSplit += StateOnUndoSplit;
    }

    /// <inheritdoc/>
    /// <param name="mode"><see cref="LayoutMode"/> passed by LiveSplit</param>
    /// <remarks>
    ///     Returned object must contain at least an empty <see cref="TableLayoutPanel"/>, otherwise the Layout Settings menu doesn't show up!
    /// </remarks>
    public override Control GetSettingsControl(LayoutMode mode) => _splitter.Settings;

    /// <inheritdoc/>
    /// <param name="document"><see cref="XmlDocument"/> passed by LiveSplit</param>
    /// <remarks>
    ///     Even if you don't have any settings, you can't return with null.
    ///     If you do, LiveSplit spams the Event Viewer with <c>Object reference not set to an instance of an object.</c> error messages.
    /// </remarks>
    /// <example>
    ///     https://github.com/LiveSplit/LiveSplit.ScriptableAutoSplit/blob/7e5a6cbe91569e7688fdb37446d32326b4b14b1c/ComponentSettings.cs#L70
    ///     https://github.com/CapitaineToinon/LiveSplit.DarkSoulsIGT/blob/master/LiveSplit.DarkSoulsIGT/UI/DSSettings.cs#L25
    /// </example>
    public override XmlNode GetSettings(XmlDocument document)
    {
        var settingsNode = document.CreateElement("Settings");
        _ = settingsNode.AppendChild(SettingsHelper.ToElement(document, nameof(_splitter.Settings.FullGame), _splitter.Settings.FullGame));
        _ = settingsNode.AppendChild(SettingsHelper.ToElement(document, nameof(_splitter.Settings.Deathrun), _splitter.Settings.Deathrun));
        return settingsNode;
    }

    /// <inheritdoc/>
    /// <param name="settings"><see cref="XmlNode"/> passed by LiveSplit</param>
    /// <remarks>
    ///     This might happen more than once (e.g. when the settings dialog is cancelled, to restore previous settings).
    ///     The XML file is the <c>[game - category].lss</c> file in your LiveSplit folder.
    /// </remarks>
    /// <example><inheritdoc cref="GetSettings"/></example>
    public override void SetSettings(XmlNode settings)
    {
        _splitter.Settings.FullGame = settings["FullGame"]?.InnerText == "True";
        _splitter.Settings.Deathrun = settings["Deathrun"]?.InnerText == "True";

        if (_splitter.Settings.FullGame)
            _splitter.Settings.FullGameModeButton.Checked = true;
        else if (_splitter.Settings.Deathrun)
            _splitter.Settings.DeathrunModeButton.Checked = true;
        else
            _splitter.Settings.ILModeButton.Checked = true;
    }
    
    public override void Dispose()
    {
        _state.OnSplit -= StateOnSplit;
        _state.OnStart -= StateOnStart;
        _state.OnUndoSplit -= StateOnUndoSplit;
        _splitter?.Dispose();
    }

    public override string ComponentName => "Classic Tomb Raider Component";

    /// <summary>
    ///     Adds <see cref="ClassicGameData"/> and <see cref="ClassicAutosplitter"/> management to <see cref="AutoSplitComponent.Update"/>.
    /// </summary>
    /// <param name="invalidator"><see cref="IInvalidator"/> passed by LiveSplit</param>
    /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
    /// <param name="width">width passed by LiveSplit</param>
    /// <param name="height">height passed by LiveSplit</param>
    /// <param name="mode"><see cref="LayoutMode"/> passed by LiveSplit</param>
    /// <remarks>
    ///     This override allows <see cref="ClassicAutosplitter"/> to use <see cref="ClassicGameData"/> in its logic.
    /// </remarks>
    public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
    {
        if (_splitter.Data.Update())
            base.Update(invalidator, state, width, height, mode);
    }
}