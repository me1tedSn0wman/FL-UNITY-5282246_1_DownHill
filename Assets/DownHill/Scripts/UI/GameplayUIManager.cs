using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : Singleton<GameplayUIManager>
{
    [SerializeField] float speedometerAnchorY = 0.2f;

    [Header("Buttons")]

    public Button settingsButton;
    public Button settingsButtonClose;

    public Button toMainMenuButton;
    public Button restartButton;

    public Button toMainMenuSettingsButton;
    public Button restartSettingsButton;

    [Header("Canvas")]

    public GameObject canvasSettingsGO;
    public GameObject canvasMobileControlGO;
    public GameObject canvasMobileJoystickGO;

    public GameObject canvasGameOverGO;
    public RectTransform canvasSpeedometerRectTransf;

    [Header("Text Meshes")]

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textScoreGame;
    public TextMeshProUGUI textTimeGame;

    public GameObject textVictory;

    [Header("Sliders")]

    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    [Header("Speedometer")]

    public SpeedometerUI speedometerUI;

    [Header("UI Audio clip")]

    public AudioClip audioClipUI;
    public AudioSource audioSourceUI;

    public void Awake()
    {
  
        settingsButton.onClick.AddListener(() => 
        {
            audioSourceUI.PlayOneShot(audioClipUI);
            ShowSettingsMenu(true);
        });
        settingsButtonClose.onClick.AddListener(() =>
        {
            audioSourceUI.PlayOneShot(audioClipUI);
            ShowSettingsMenu(false);
        });

        PlayerControl.Instance.OnPauseButtonPressed += OnPauseButtonPressed;


        toMainMenuButton.onClick.AddListener(() =>
        {
            ShowSettingsMenu(false);
            audioSourceUI.PlayOneShot(audioClipUI);
            GameManager.Instance.LoadMainScene();

        });
        toMainMenuSettingsButton.onClick.AddListener(() =>
        {
            ShowSettingsMenu(false);
            audioSourceUI.PlayOneShot(audioClipUI);
            GameManager.Instance.LoadMainScene();
        });



        restartButton.onClick.AddListener(() => {
            ShowSettingsMenu(false);
            audioSourceUI.PlayOneShot(audioClipUI);
            GameManager.Instance.ReloadLevel();
        });
        restartSettingsButton.onClick.AddListener(() =>
        {
            ShowSettingsMenu(false);
            audioSourceUI.PlayOneShot(audioClipUI);
            GameManager.Instance.ReloadLevel();
        });

        musicVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.Instance.musicVolume = value;
        });
        soundVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.Instance.soundVolume = value;
        });



        musicVolumeSlider.value = GameManager.Instance.musicVolume;
        soundVolumeSlider.value = GameManager.Instance.soundVolume;

        canvasSettingsGO.SetActive(false);
        canvasGameOverGO.SetActive(false);

        UpdateScore(0);
        ActivateMobileCanvases(GameManager.Instance.IS_MOBILE);
    }

    public void ActivateMobileCanvases(bool value) {
        settingsButton.gameObject.SetActive(value);
        canvasMobileControlGO.SetActive(value);
        canvasMobileJoystickGO.SetActive(value);
        if (!value)
        {
            float speedometerAnchCrntXmin = canvasSpeedometerRectTransf.anchorMin.x;
            float speedometerAnchCrntXmax = canvasSpeedometerRectTransf.anchorMax.x;
            canvasSpeedometerRectTransf.anchorMin = new Vector2(speedometerAnchCrntXmin, speedometerAnchorY);
            canvasSpeedometerRectTransf.anchorMax = new Vector2(speedometerAnchCrntXmax, speedometerAnchorY);
            Vector3 crntPos = canvasSpeedometerRectTransf.localPosition;
//            Debug.Log(crntPos);
//            canvasSpeedometerRectTransf.localPosition = new Vector3(crntPos.x, 0, crntPos.z);
        }
    }

    public void ActivateGameOverUI(bool isVictory) {
        textVictory.SetActive(isVictory);
        textScore.text = GameplayManager.Instance.score.ToString();
        canvasGameOverGO.SetActive(true);
    }

    public void SetSpeedometerUIVal(float value) {
        speedometerUI.SetValue(value);
    }

    public void OnPauseButtonPressed()
    {
        if (canvasSettingsGO.active)
            ShowSettingsMenu(false);
        else
            ShowSettingsMenu(true);
    }

    public void ShowSettingsMenu(bool value) {
        canvasSettingsGO.SetActive(value);
        if (value)
            Time.timeScale = 0;
        else {
            Time.timeScale = 1;
        }
    }

    public void UpdateScore(int score) {
        textScoreGame.text = score.ToString();
    }

    public void UpdateTimer(float time)
    {

        int time_min =Mathf.FloorToInt(time) / 60;
        int time_sec = Mathf.FloorToInt(time) % 60;
        int time_mil = Mathf.FloorToInt(time * 100) % 100;
        string time_str = string.Format("{0:d2}:{1:d2}.{2:d2}",time_min,time_sec,time_mil);
        textTimeGame.text = time_str;
    }

    public override void OnDestroy() {
        PlayerControl.Instance.OnPauseButtonPressed -= OnPauseButtonPressed;
        base.OnDestroy();
    }
}

