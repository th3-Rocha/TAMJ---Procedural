using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    public Toggle AsyncMeshGen;
    public Toggle PostProcessing;
    public Toggle Vsync;
    public MapGenerator mapGenerator;
    public Camera mainCamera;

    public Slider DrawDistance;
    private UniversalAdditionalCameraData cameraData;

    void Start()
    {
        mainCamera = Camera.main;
        cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        pauseMenuUI.SetActive(false);

        // Initialize toggles to reflect current settings
        Vsync.isOn = QualitySettings.vSyncCount > 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        mapGenerator.isAsyncGen = AsyncMeshGen.isOn;
        mapGenerator.terrainDistView = (int)DrawDistance.value;

        // Apply post-processing setting
        cameraData.renderPostProcessing = PostProcessing.isOn;

        // Apply V-Sync setting
        QualitySettings.vSyncCount = Vsync.isOn ? 1 : 0; // 1 enables V-Sync, 0 disables it
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;

        AsyncMeshGen.isOn = mapGenerator.isAsyncGen;
        DrawDistance.value = mapGenerator.terrainDistView;

        // Reflect the current post-processing state
        PostProcessing.isOn = cameraData.renderPostProcessing;

        // Reflect the current V-Sync state
        Vsync.isOn = QualitySettings.vSyncCount > 0;
    }
}
