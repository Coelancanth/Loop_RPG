using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public static SceneSetup Instance { get; private set; }

    [Header("Core Systems")]
    public GameManager GameManager;
    public TileSystem TileSystem;
    public ResourceManager ResourceManager;
    public LoopCounter LoopCounter;
    [Header("Input")]
    public TilePlacer TilePlacer;
    public Camera GameCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupScene()
    {
        // 确保有主相机
        if (GameCamera == null)
        {
            GameCamera = Camera.main;
            if (GameCamera == null)
            {
                var cameraObj = new GameObject("Main Camera");
                GameCamera = cameraObj.AddComponent<Camera>();
                cameraObj.tag = "MainCamera";
                GameCamera.orthographic = true;
                GameCamera.orthographicSize = 5f;
                GameCamera.transform.position = new Vector3(0, 0, -10);
            }
        }

        // 设置TilePlacer引用
        if (TilePlacer != null)
        {
            TilePlacer.SetupReferences(TileSystem, GameCamera);
        }

        // 设置PathSystem引用
        if (TileSystem != null)
        {
            TileSystem.SetupReferences(ResourceManager);
        }
    }
} 