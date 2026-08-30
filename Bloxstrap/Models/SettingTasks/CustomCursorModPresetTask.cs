using Bloxstrap.Models.SettingTasks.Base;

namespace Bloxstrap.Models.SettingTasks
{
    public class CustomCursorModPresetTask : StringBaseTask
    {
        private static readonly string[] CursorPaths = new[]
        {
            @"content\textures\Cursors\KeyboardMouse\ArrowCursor.png",
            @"content\textures\Cursors\KeyboardMouse\ArrowFarCursor.png"
        };

        public CustomCursorModPresetTask() : base("ModPreset", "CustomCursor")
        {
            if (File.Exists(GetCursorTargetPath()))
                OriginalState = GetCursorTargetPath();
        }

        private static string GetCursorTargetPath() => Path.Combine(Paths.Modifications, CursorPaths[0]);

        public override void Execute()
        {
            foreach (string relativePath in CursorPaths)
            {
                string targetPath = Path.Combine(Paths.Modifications, relativePath);

                if (!String.IsNullOrEmpty(NewState) && File.Exists(NewState))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);

                    Filesystem.AssertReadOnly(targetPath);
                    File.Copy(NewState, targetPath, true);
                }
                else if (File.Exists(targetPath))
                {
                    Filesystem.AssertReadOnly(targetPath);
                    File.Delete(targetPath);
                }
            }

            OriginalState = NewState;
        }
    }
}