using TRUtil;

namespace TR5;

public sealed class ComponentSettings : LaterClassicComponentSettings
{
    public ComponentSettings()
    {
        SuspendLayout();
        OptionCheckbox.Checked = true;
        OptionCheckbox.Name = "SplitCutsceneCheckbox";
        OptionCheckbox.Text = "Split Security Breach Cutscene";
        ResumeLayout(false);
        PerformLayout();
    }

    public override void SetGameVersion(uint version)
    {
        const string digitalText = "Steam/GOG [TR5]";
        const string unknownText = "Unknown/Undetected";

        string versionText = (GameVersion) version switch
        {
            GameVersion.SteamOrGog => digitalText,
            _ => unknownText
        };
        GameVersionLabel.Text = "Game Version: " + versionText;
    }
}