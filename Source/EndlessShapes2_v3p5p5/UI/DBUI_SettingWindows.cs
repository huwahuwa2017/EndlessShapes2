using BrilliantSkies.Ui.Consoles;

namespace EndlessShapes2.UI
{
    public class DBUI_SettingWindows : ConsoleUi<DecorationBuilder>
    {
        private ConsoleWindow consoleWindow_0;

        private ConsoleWindow consoleWindow_1;

        public DBUI_SettingWindows(DecorationBuilder focus) : base(focus)
        {
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            consoleWindow_0 = NewWindow(0, "Basic Setting", new ScaledRectangle(200f, 100f, 400f, 600f));
            consoleWindow_0.DisplayTextPrompt = false;
            consoleWindow_0.SetMultipleTabs(new DBUI_BasicSettingTab(consoleWindow_0, _focus, this));

            consoleWindow_1 = NewWindow(1, "Advanced Setting", new ScaledRectangle(680f, 100f, 400f, 600f));
            consoleWindow_1.DisplayTextPrompt = false;
            consoleWindow_1.SetMultipleTabs(new DBUIAdvancedSettingTab(consoleWindow_1, _focus));

            return consoleWindow_0;
        }
    }
}
