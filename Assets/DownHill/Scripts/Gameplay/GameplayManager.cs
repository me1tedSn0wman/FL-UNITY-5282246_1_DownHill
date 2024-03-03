using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameplayManager : Singleton<GameplayManager>
{
    public int chosenCarId;
    public GameObject crntCar;
    public CinemachineVirtualCamera virtCam;
    public Vector3 gravityScene;

    public Transform spawnAnchor;

    private int _score;

    [Header("UI Audio clip")]

    public AudioClip audioClipPickUp;
    public AudioSource audioSourceSounds;

    public int timeScoreInitialCount = 600;

    public float timeLevelStart = -1;

    public int score {
        get { return _score; }
        set { _score = value; }
    }


    public void Awake()
    {
        LoadChosenCar();
        SetCameraToSwawnedCar();
        Physics.gravity = gravityScene;
        timeLevelStart = Time.time;
    }

    public void LoadChosenCar() {
        chosenCarId = GameManager.Instance.chosenCarId;
        CarReference carReference = GameManager.Instance.dictOfCars[chosenCarId];
        crntCar = Instantiate(carReference.carPrefab, spawnAnchor);
    }

    public void SetCameraToSwawnedCar() {
        virtCam.Follow = crntCar.transform;
        virtCam.LookAt = crntCar.transform.Find("Aim");
//        virtCam.gameObject.transform.SetParent(crntCar.transform);
    }

    public void AddScore(int addValue) {
        score += addValue;
        GameplayUIManager.Instance.UpdateScore(score);
    }

    public void FinishLevel(bool isVictory) {
        if (isVictory) {
            int scoreTime = timeScoreInitialCount - 2 * Mathf.RoundToInt(Time.time - timeLevelStart);
            AddScore(Mathf.Max(scoreTime, 0));
            }
        GameplayUIManager.Instance.ActivateGameOverUI(isVictory);
        GameManager.Instance.gameState = GameState.GameOver;
        GameManager.Instance.FinishLevel(score);
    }

    public void PlayPickUpSound() {
        audioSourceSounds.PlayOneShot(audioClipPickUp);
    }

    public void Update()
    {
        UpdateTimer();
    }

    public void UpdateTimer() {
        float levelTime = Time.time - timeLevelStart;
        GameplayUIManager.Instance.UpdateTimer(levelTime);
    }
}
