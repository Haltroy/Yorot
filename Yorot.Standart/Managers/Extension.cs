namespace Yorot
{
    public class Extensions
    {
        public Settings Settings { get; set; }
        public string ToXml()
        {

        }
        public void Save()
        {
            HTAlt.Tools.WriteFile(Settings.UserExt, ToXml(), System.Text.Encoding.Unicode);
        }
    }

    public class Extension
    {

    }
}
