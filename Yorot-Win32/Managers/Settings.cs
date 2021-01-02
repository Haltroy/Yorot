namespace Yorot
{
    public class Settings
    {
        public Settings()
        {

        }

        public AppMan AppMan { get; set; } = new AppMan(YorotGlobal.UserApp);
    }
}
