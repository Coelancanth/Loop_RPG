using UnityEngine;
using System.Collections.Generic;

namespace Game.Path.Testing
{
    public class PathVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private float tileSize = 1f;
        [SerializeField] private Color pathColor = Color.white;
        [SerializeField] private Color startColor = Color.green;
        [SerializeField] private Color endColor = Color.red;

        private List<GameObject> pathTiles = new List<GameObject>();

        public void VisualizePath(List<Vector2Int> path)
        {
            if (path == null || path.Count == 0)
                return;

            ClearPath();

            for (int i = 0; i < path.Count; i++)
            {
                var position = new Vector3(path[i].x * tileSize, path[i].y * tileSize, 0);
                var tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                
                if (spriteRenderer != null)
                {
                    // Set color based on position in path
                    if (i == 0)
                        spriteRenderer.color = startColor;
                    else if (i == path.Count - 1)
                        spriteRenderer.color = endColor;
                    else
                        spriteRenderer.color = pathColor;
                }

                pathTiles.Add(tile);
            }

            // Center the camera on the path
            CenterCameraOnPath(path);
        }

        public void ClearPath()
        {
            foreach (var tile in pathTiles)
            {
                if (tile != null)
                    Destroy(tile);
            }
            pathTiles.Clear();
        }

        private void CenterCameraOnPath(List<Vector2Int> path)
        {
            if (path == null || path.Count == 0)
                return;

            // Calculate bounds
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            foreach (var point in path)
            {
                min.x = Mathf.Min(min.x, point.x * tileSize);
                min.y = Mathf.Min(min.y, point.y * tileSize);
                max.x = Mathf.Max(max.x, point.x * tileSize);
                max.y = Mathf.Max(max.y, point.y * tileSize);
            }

            // Calculate center
            Vector2 center = (min + max) * 0.5f;

            // Move camera to center
            var camera = Camera.main;
            if (camera != null)
            {
                camera.transform.position = new Vector3(center.x, center.y, camera.transform.position.z);

                // Adjust orthographic size to fit the path
                float width = max.x - min.x + tileSize * 2;
                float height = max.y - min.y + tileSize * 2;
                float aspect = camera.aspect;
                camera.orthographicSize = Mathf.Max(height * 0.5f, width * 0.5f / aspect);
            }
        }
    }
} 