using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using YG;
using static Cinemachine.DocumentationSortingAttribute;

public enum GameState { 
    PreGame,
    MainMenu,
    Gameplay,
    GameOver,
    FadeInOut,
}

public class GameManager : Soliton<GameManager>
{
    public bool IS_MOBILE;
    public static bool IS_ADMIN;
    public static int SAVE_ARRAY_LENGTH = 15;

    private GameState _gameState;
    public GameState gameState
    {
        get { return _gameState; }
        set {
            _gameState = value;
        }
    }

    public List<SceneReference> listOfScenes;
    public Dictionary<int, SceneReference> dictOfLevels;


    public CarReferenceConfigurator carReferenceConfigurator;
    public List<CarReference> listOfCars;
    public Dictionary<int, CarReference> dictOfCars;

    public int[] highScores;

    public bool isAdmin;
    public bool simulateMobile;

    public int chosenCarId = 1;
    public int crntLevelId = 1;

    Action LoadLevelAction;

    public int pointsCount;


    public List<int> unlockedCars;
    public List<int> unlockedLevels;

    [Header("Settings")]
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _soundVolume;
    public float musicVolume {
        get { return _musicVolume; }
        set {
            _musicVolume = Mathf.Clamp01(value);
            OnMusicVolumeChanged(_musicVolume);
        }
    }
    public float soundVolume {
        get { return _soundVolume; }
        set
        {
            _soundVolume = Mathf.Clamp01(value);
            OnSoundVolumeChanged(_soundVolume);
        }
    }
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSoundVolumeChanged;

    public override void Awake()
    {
        base.Awake();
        gameState = GameState.PreGame;

        IS_ADMIN = isAdmin;
        IS_MOBILE = false
            || YandexGame.EnvironmentData.isMobile
            || YandexGame.EnvironmentData.isTablet
            || simulateMobile;

        listOfCars = carReferenceConfigurator.listOfCarReferences;
        dictOfLevels = new Dictionary<int, SceneReference>();
        dictOfCars = new Dictionary<int, CarReference>();
        highScores = new int[15];

        LoadLevelAction = () => ActionsOnLoadLevel();


        MakeLevelDict();
        MakeCarDict();

        SceneManager.sceneLoaded += ActionsOnFadeIn;

        gameState = GameState.MainMenu;

        StartCoroutine(initMusic());
    }

    #region PREGAME

    public void MakeLevelDict() {
        for (int i = 0; i < listOfScenes.Count; i++)
        {
            SceneReference crntScRef = listOfScenes[i];
            if (!dictOfLevels.ContainsKey(crntScRef.levelId))
            {
                dictOfLevels.Add(crntScRef.levelId, crntScRef);
            }
        }
    }

    public void MakeCarDict() {
        for (int i = 0; i < listOfCars.Count; i++)
        {
            CarReference crntCarRef = listOfCars[i];
            if (!dictOfCars.ContainsKey(crntCarRef.carId))
            {
                dictOfCars.Add(crntCarRef.carId, crntCarRef);
                if (!crntCarRef.isBlocked)
                    unlockedCars.Add(crntCarRef.carId);
            }
        }
    }

    #endregion PREGAME

    #region SceneManagment

    void ActionsOnFadeIn(Scene scene, LoadSceneMode mode)
    {
        FadeScreen.Instance.FadeIn(LoadLevelAction);
    }

    public void ActionsOnFadeIn()
    {
        FadeScreen.Instance.FadeIn(LoadLevelAction);
    }

    public void ActionsOnLoadLevel()
    {
        if (crntLevelId == -1)
            gameState = GameState.MainMenu;
        if (crntLevelId != -1)
            gameState = GameState.Gameplay;
    }

    public void LoadLevel(int levelId)
    {
        gameState = GameState.FadeInOut;
        SceneReference sceneToLoad;
//        Debug.Log(levelId);
        if (!dictOfLevels.TryGetValue(levelId, out sceneToLoad)) return;
        crntLevelId = levelId;
        sceneToLoad.ReloadLevel(FadeScreen.Instance);
    }

    public void LoadMainScene()
    {
        gameState = GameState.FadeInOut;
        crntLevelId = -1;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void ReloadLevel() {
        LoadLevel(crntLevelId);
    }

    #endregion SceneManagment

    #region GameThings

    public void FinishLevel(int score) {
        pointsCount += score;
        if (score > highScores[crntLevelId]) {
            highScores[crntLevelId] = score;
        }
        Save();
    }

    public void AddPoints(int addValue) {
        pointsCount += addValue;
        Save();
    }

    public bool TrySpendPoints(int value) {
        if (pointsCount < value) return false;
        pointsCount -= value;
        return true;
    }

    public void UnlockCar(int carId) {
        if (unlockedCars.Contains(carId)) return;
        unlockedCars.Add(carId);
        Save();
    }

    public void UnlockLevel(int levelId) {
        if (unlockedLevels.Contains(levelId)) return;
        unlockedLevels.Add(levelId);
    }

    public bool TryUnlockLevel(int levelId) {
        if (!TrySpendPoints(dictOfLevels[levelId].levelPrice)) return false;
        UnlockLevel(levelId);
        Save();
        return true;
    }

    public bool IsCarBlocked(int carId) {
        return !unlockedCars.Contains(carId);
    }

    #endregion GameThings

    #region SAVES

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    public void Save() {
        YandexGame.savesData.pointsCount = pointsCount;
        for (int i = 0; i < unlockedLevels.Count && i < SAVE_ARRAY_LENGTH; i++) {
            YandexGame.savesData.openLevels[i] = unlockedLevels[i];
        }
        for (int i = 0; i < unlockedCars.Count && i < SAVE_ARRAY_LENGTH; i++)
        {
            YandexGame.savesData.openCars[i] = unlockedCars[i];
        }
        for (int i = 0; i < listOfScenes.Count && i < SAVE_ARRAY_LENGTH; i++)
        {
            YandexGame.savesData.highScores[i] = highScores[i];
        }
        YandexGame.SaveProgress();
    }

    public void Load() => YandexGame.LoadProgress();

    public void GetLoad() {
        pointsCount = 0;
        unlockedLevels = new List<int>();
        unlockedCars = new List<int>();
        highScores = new int[15];

        pointsCount = YandexGame.savesData.pointsCount;
        for (int i = 0; i < SAVE_ARRAY_LENGTH; i++)
        {
            if (YandexGame.savesData.openLevels[i] == 0) break;
            unlockedLevels.Add(YandexGame.savesData.openLevels[i]);
        }
        for (int i = 0; i < SAVE_ARRAY_LENGTH; i++)
        {
            if (YandexGame.savesData.openCars[i] == 0) break;
            unlockedCars.Add(YandexGame.savesData.openCars[i]);
        }
        for (int i = 0; i < SAVE_ARRAY_LENGTH; i++) {
            highScores[i] = YandexGame.savesData.highScores[i];
        }

        MainMenuUIManager.Instance.UpdateScoreText();
        MainMenuUIManager.Instance.UpdateLevelList();
        MainMenuSceneManager.Instance.UpdateCarList();
    }

    #endregion SAVES

    private IEnumerator initMusic() {
        yield return new WaitForSeconds(2f);
        musicVolume = 0.5f;
        soundVolume = 0.2f;
    }
}
