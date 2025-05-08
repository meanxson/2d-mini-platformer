using System;

namespace Client
{
    public class MainMenuView : BaseWindowsManager
    {
        private void Start()
        {
            OpenWindow<MainMenuWindow>();
        }
    }
}