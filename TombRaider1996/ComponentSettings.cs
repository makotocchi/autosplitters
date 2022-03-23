using TRUtil;

namespace TR1
{
    public sealed class ComponentSettings : ClassicComponentSettings
    {
        public override void SetGameVersion(uint version)
        {
            string versionText;
            switch ((GameVersion)version)
            {
                case GameVersion.DOSBox:
                    versionText = "DOSBox [TR 1996]";
                    break;
                case GameVersion.Ati:
                    versionText = "TombATI [TR 1996]";
                    break;
                case GameVersion.AtiUnfinishedBusiness:
                    versionText = "TombATI [TR:UB]";
                    break;
                default:
                    versionText = "Unknown/Undetected";
                    break;
            }
            GameVersionLabel.Text = "Game Version: " + versionText;
        }
    }
}
