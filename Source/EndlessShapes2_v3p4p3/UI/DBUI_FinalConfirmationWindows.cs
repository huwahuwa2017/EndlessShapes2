using BrilliantSkies.Ui.Consoles;

namespace EndlessShapes2.UI
{
    public class DBUI_FinalConfirmationWindows : ConsoleUi<DecorationBuilder>
    {
        private ConsoleWindow consoleWindow_0;

        public DBUI_FinalConfirmationWindows(DecorationBuilder focus) : base(focus)
        {
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            consoleWindow_0 = NewWindow(0, "Build", new ScaledRectangle(350f, 100f, 580f, 600f));
            consoleWindow_0.DisplayTextPrompt = false;
            consoleWindow_0.SetMultipleTabs(new DBUI_FinalConfirmationTab(consoleWindow_0, _focus, this));

            return consoleWindow_0;
        }
    }
}
