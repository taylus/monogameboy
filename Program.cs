using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoGameBoy
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var scenes = GetScenes();
            PrintScenes(scenes);
            int selectedSceneNumber = GetSceneChoice(scenes);
            var selectedSceneName = scenes[selectedSceneNumber];
            RunGame(selectedSceneName);
        }

        private static IList<string> GetScenes()
        {
            var sceneDir = new DirectoryInfo("input");
            var files = sceneDir.GetFiles("*.bin");
            return files.Select(f => f.Name.WithoutFileExtension()).Distinct().ToList();
        }

        private static string WithoutFileExtension(this string path) => path.Split('.')[0];

        private static void PrintScenes(IList<string> scenes)
        {
            Console.WriteLine("Select scene:");
            for (int i = 0; i < scenes.Count; i++)
            {
                Console.WriteLine($" {i + 1}.) {scenes[i]}");
            }
        }

        private static int GetSceneChoice(IList<string> scenes)
        {
            int? selectedScene = null;
            while (!selectedScene.HasValue || selectedScene.Value < 1 || selectedScene.Value > scenes.Count)
            {
                Console.Write("Select a scene: ");
                selectedScene = TryReadLineAsInt();
            }
            return selectedScene.Value - 1; //scene #s displayed starting at 1
        }

        private static int? TryReadLineAsInt()
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out int parsed)) return parsed;
            return null;
        }

        private static void RunGame(string sceneName)
        {
            using (var game = new MonoGameBoy(sceneName))
                game.Run();
        }
    }
}
