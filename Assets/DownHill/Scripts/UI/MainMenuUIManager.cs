using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    [Header("Buttons")]

    public Button settingsButton;
    public Button infoButton;
    public Button levelSelectButton;

    public Button closeSettingsButton;
    public Button closeInfoButton;
    public Button closeLevelSelectButton;

    public Button prevCarButton;
    public Button nextCarButton;
    public Button unlcokCarButton;

    [Header("Buttons Admin")]

    public Button addPointsButton;

    public Button saveGMButton;
    public Button loadGMButton;

    public GameObject isMobileTextGO;
    public GameObject isTabletTextGO;
    public GameObject isDesktopTextGO;

    [Header("Canvases")]

    public GameObject settignsCanvasGO;
    public GameObject infoCanvasGO;
    public GameObject levelSelectCanvasGO;
    public GameObject levelSelectContentGO;

    [Header("Prefabs")]

    public LevelIcon levelIconPrefab;

    [Header("Textes")]
    public TextMeshProUGUI textPointsCount;
    public TextMeshProUGUI textCarPrice;

    [Header("Images")]

    public GameObject imageLockCarGO;
    public GameObject imageLockCarBackgroudGO;
    public GameObject levelSelectLockGO;

    [Header("Sliders")]

    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    [Header("UI Audio clip")]

    public AudioClip audioClipUI;
    public AudioSource audioSourceUI;

    [Header("Set Dynamicaly")]
    public List<LevelIcon> levelIcons;

    public void Awake()
    {
        settingsButton.onClick.AddListener(() =>
        {
            settignsCanvasGO.SetActive(true);
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        infoButton.onClick.AddListener(() =>
        {
            infoCanvasGO.SetActive(true);
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        levelSelectButton.onClick.AddListener(() =>
        {
            levelSelectCanvasGO.SetActive(true);
            audioSourceUI.PlayOneShot(audioClipUI);
        });


        closeSettingsButton.onClick.AddListener(() =>
        {
            settignsCanvasGO.SetActive(false);
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        closeInfoButton.onClick.AddListener(() =>
        {
            infoCanvasGO.SetActive(false);
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        closeLevelSelectButton.onClick.AddListener(() =>
        {
            levelSelectCanvasGO.SetActive(false);
            audioSourceUI.PlayOneShot(audioClipUI);
        });

        prevCarButton.onClick.AddListener(() => {
            MainMenuSceneManager.Instance.ShowPrevCar();
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        nextCarButton.onClick.AddListener(() => {
            MainMenuSceneManager.Instance.ShowNextCar();
            audioSourceUI.PlayOneShot(audioClipUI);
        });
        unlcokCarButton.onClick.AddListener(() => {
            TryUnlockCar();
            audioSourceUI.PlayOneShot(audioClipUI);
        });


        ///Admin things
        addPointsButton.onClick.AddListener(() => {
            GameManager.Instance.AddPoints(2000);
            UpdateScoreText();
        });
        saveGMButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Save();
            UpdateScoreText();
        });
        loadGMButton.onClick.AddListener(() => {
            GameManager.Instance.Load();
            UpdateScoreText();
        });


        //sound

        musicVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.Instance.musicVolume = value;
        });
        soundVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.Instance.soundVolume = value;
        });

        musicVolumeSlider.value = GameManager.Instance.musicVolume;
        soundVolumeSlider.value = GameManager.Instance.soundVolume;

        UpdateScoreText();
        SetLevelList();
        ActivateAdminThings(GameManager.IS_ADMIN);


        /*
         */
        if (YandexGame.EnvironmentData.isMobile && GameManager.IS_ADMIN) isMobileTextGO.SetActive(true);
        if (YandexGame.EnvironmentData.isTablet && GameManager.IS_ADMIN) isTabletTextGO.SetActive(true);
        if (YandexGame.EnvironmentData.isDesktop && GameManager.IS_ADMIN) isDesktopTextGO.SetActive(true);

        /*
         */
    }

    public void SetLevelList() {
        if (GameManager.Instance.dictOfLevels.Count <= 0)
        {
            throw new UnityException("there is no levels");
            return;
        }

        foreach (KeyValuePair<int, SceneReference> kvp in GameManager.Instance.dictOfLevels) {

            LevelIcon crntLevelIcon = Instantiate(levelIconPrefab, levelSelectContentGO.transform);
            bool crntIsLevelBlocked = kvp.Value.isLevelBlocked;
            if (GameManager.Instance.unlockedLevels.Contains(kvp.Key)) 
                crntIsLevelBlocked = false;

            crntLevelIcon.SetLevelIconValues(
                kvp.Value.levelName
                , kvp.Value.levelId
                , crntIsLevelBlocked
                , kvp.Value.levelPrice
                , GameManager.Instance.highScores[kvp.Key]
                , kvp.Value.levelSprite
                );
            levelIcons.Add(crntLevelIcon);
        }
    }

    public void UpdateLevelList() {
        foreach (LevelIcon levelIcon in levelIcons) {
            if (GameManager.Instance.unlockedLevels.Contains(levelIcon.levelId)) {
                levelIcon.isBlocked = false;
            }
        }
        for (int i = 0; i < GameManager.SAVE_ARRAY_LENGTH && i<levelIcons.Count; i++) {
            levelIcons[i].UpdateLevelIconValues(GameManager.Instance.highScores[i]);
        }

    }


    public void UpdateScoreText() {
        textPointsCount.text = GameManager.Instance.pointsCount.ToString();
    }

    public void ActivateAdminThings(bool value)
    {
        addPointsButton.gameObject.SetActive(value);
        saveGMButton.gameObject.SetActive(value);
        loadGMButton.gameObject.SetActive(value);
    }

    public void SetCarLockUI(bool value, int price) {
        unlcokCarButton.gameObject.SetActive(value);
        imageLockCarGO.SetActive(value);
        imageLockCarBackgroudGO.SetActive(value);
        levelSelectLockGO.SetActive(value);
        textCarPrice.text = price.ToString();
    }

    public void TryUnlockCar() {
        if (MainMenuSceneManager.Instance.TryUnlockCar()) 
            SetCarLockUI(false, 0);
        UpdateScoreText();
    }
}
