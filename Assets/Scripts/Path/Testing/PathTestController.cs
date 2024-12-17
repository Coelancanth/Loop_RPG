using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace Game.Path.Testing
{
    public class PathTestController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PathVisualizer visualizer;
        [SerializeField] private TMP_Dropdown characterClassDropdown;
        [SerializeField] private TMP_InputField pathLengthInput;
        [SerializeField] private Button generateButton;
        [SerializeField] private TextMeshProUGUI statsText;

        [Header("Settings")]
        [SerializeField] private int defaultPathLength = 30;
        [SerializeField] private int randomSeed = 42;

        private PathInitializer pathInitializer;
        private CharacterClass currentClass = CharacterClass.Default;
        private int currentLength;

        private void Start()
        {
            // Initialize RandomManager singleton with seed
            RandomManager.Instance.Initialize(randomSeed);

            // Initialize PathInitializer with the singleton instance
            pathInitializer = new PathInitializer(RandomManager.Instance);

            // Setup UI
            SetupUI();

            // Generate initial path
            GenerateNewPath();
        }

        private void SetupUI()
        {
            // Setup character class dropdown
            if (characterClassDropdown != null)
            {
                characterClassDropdown.ClearOptions();
                characterClassDropdown.AddOptions(System.Enum.GetNames(typeof(CharacterClass)).ToList());
                characterClassDropdown.onValueChanged.AddListener(OnCharacterClassChanged);
            }

            // Setup path length input
            if (pathLengthInput != null)
            {
                pathLengthInput.text = defaultPathLength.ToString();
                pathLengthInput.onEndEdit.AddListener(OnPathLengthChanged);
            }

            // Setup generate button
            if (generateButton != null)
            {
                generateButton.onClick.AddListener(GenerateNewPath);
            }

            currentLength = defaultPathLength;
        }

        private void OnCharacterClassChanged(int index)
        {
            currentClass = (CharacterClass)index;
            GenerateNewPath();
        }

        private void OnPathLengthChanged(string value)
        {
            if (int.TryParse(value, out int length) && length >= 4)
            {
                currentLength = length;
                GenerateNewPath();
            }
            else
            {
                // Reset to previous valid value
                pathLengthInput.text = currentLength.ToString();
            }
        }

        public void GenerateNewPath()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var path = pathInitializer.GenerateInitialPath(currentClass, currentLength);
                
                stopwatch.Stop();

                if (path != null && path.Count > 0)
                {
                    visualizer.VisualizePath(path);
                    UpdateStats(path, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    UnityEngine.Debug.LogError("Generated path is null or empty");
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to generate path: {e.Message}\nStackTrace: {e.StackTrace}");
                if (statsText != null)
                {
                    statsText.text = $"Error: {e.Message}";
                }
            }
        }

        private void UpdateStats(System.Collections.Generic.List<Vector2Int> path, long elapsedMs)
        {
            if (statsText == null || path == null)
                return;

            var stats = new StringBuilder();
            stats.AppendLine($"Character Class: {currentClass}");
            stats.AppendLine($"Path Length: {path.Count}");
            stats.AppendLine($"Generation Time: {elapsedMs}ms");
            stats.AppendLine($"Random Seed: {RandomManager.Instance.CurrentSeed}");
            
            // Calculate path bounds
            Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);
            foreach (var point in path)
            {
                min.x = Mathf.Min(min.x, point.x);
                min.y = Mathf.Min(min.y, point.y);
                max.x = Mathf.Max(max.x, point.x);
                max.y = Mathf.Max(max.y, point.y);
            }

            stats.AppendLine($"Path Bounds: ({min.x},{min.y}) to ({max.x},{max.y})");
            stats.AppendLine($"Area: {(max.x - min.x + 1) * (max.y - min.y + 1)} tiles");

            statsText.text = stats.ToString();
        }
    }
} 