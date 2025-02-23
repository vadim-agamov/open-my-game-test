using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.Configs.Editor
{
    public class PuzzleInfoConfigGenerator : EditorWindow
    {
        private static readonly string SpritesDirectory = "Assets/CommonAssets/Puzzles/Sprites/PuzzlePreviews";
        private static readonly string ConfigDirectory = "Assets/CommonAssets/Puzzles/Configs";
        
        private static readonly string[] Categories = {"Nature", "Animals", "Food", "Objects", "People", "Places"};

        [MenuItem("Tools/Generate PuzzleInfoConfigs")]
        public static void GenerateConfigs()
        {
            if (!Directory.Exists(SpritesDirectory))
            {
                Debug.LogError($"Puzzle directory not found: {SpritesDirectory}");
                return;
            }

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
                AssetDatabase.Refresh();
            }

            var imageFiles = Directory.GetFiles(SpritesDirectory, "*.png");

            for (var index = 0; index < imageFiles.Length; index++)
            {
                var filePath = imageFiles[index];
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var configPath = $"{ConfigDirectory}/Puzzle_{index}.asset";

                if (AssetDatabase.LoadAssetAtPath<PuzzleInfoConfig>(configPath) != null)
                {
                    Debug.Log($"Config already exists for {fileName}, skipping...");
                    continue;
                }
                
                var config = CreateInstance<PuzzleInfoConfig>();
                config.name = fileName;
                config.SetPuzzleInfo(
                    index.ToString(), 
                    $"Puzzle {index}", 
                    Categories[index % Categories.Length],
                    new AssetReferenceSprite(AssetDatabase.AssetPathToGUID(filePath))
                    );

                AssetDatabase.CreateAsset(config, configPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}