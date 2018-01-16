using Fingerprints.ViewModels;

namespace Fingerprints
{
    public enum DialogTypes
    {
        LeftDrawing,
        RightDrawing,
        MainWindow
    }

    public static class Dialogs
    {
        public const DialogTypes LeftDrawing = DialogTypes.LeftDrawing;

        public const DialogTypes RightDrawing = DialogTypes.RightDrawing;

        public const DialogTypes MainWindow = DialogTypes.MainWindow;
    }
}
