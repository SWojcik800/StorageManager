namespace StorageManager.App.Helpers
{
    public sealed class AppManager
    {
        public static AppManager Instance;
        public string CurrentPageTitle { get; set; }


        public static void Init()
        {
            Instance = new AppManager();
        }

        public bool ShowYesNoDialog(string title, string content)
        {
            return MessageBox.Show(content, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Wystąpił błąd");
        }

        public void ShowInfoMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowDataSavedMessage()
        {
            MessageBox.Show("Zapisano dane");
        }

        public void ShowPermissionDeniedMessage()
        {
            MessageBox.Show("Brak wymaganych uprawnień", "Brak uprawnień");
        }
    }
}
