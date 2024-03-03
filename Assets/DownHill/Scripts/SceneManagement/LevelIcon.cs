using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelIcon : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private TextMeshProUGUI textLevelName;
    [SerializeField] private TextMeshProUGUI textLevelScore;
    [SerializeField] private TextMeshProUGUI textLevelPrice;

//    public Sprite levelImage;
    public Image levelImage;
    public GameObject imIsBlocked;

    [SerializeField] private Button levelIconButton;
    [SerializeField] private Button levelUnlockButton;

    [Header("Set Dynamically")]
//    private RectTransform rectTransf;

    public string levelName;
    public int levelId;

    public int levelHighScore;

    public int levelPrice;

    private bool _isBlocked;
    public bool isBlocked {
        get { return _isBlocked; }
        set {
            switch (value)
            {
                case true:
                    imIsBlocked.SetActive(true);
                    levelUnlockButton.gameObject.SetActive(true);
                    _isBlocked = value;
                    break;
                case false:
                    imIsBlocked.SetActive(false);
                    levelUnlockButton.gameObject.SetActive(false);
                    _isBlocked = value;
                    break;
            }
        }
    }

    public void Awake()
    {
 //       rectTransf = GetComponent<RectTransform>();
        levelIconButton.onClick.AddListener(() => {
            if(!isBlocked)
                GameManager.Instance.LoadLevel(levelId);
        });

        levelUnlockButton.onClick.AddListener(() => {
            TryUnlockLevel();
        });
    }

    public void SetLevelIconValues(string levelName, int levelId,bool isLevelBlocked, int levelPrice, int levelHighScore, Sprite levelImage)
    {
        this.levelName = levelName;
        this.levelId = levelId;
        this.isBlocked = isLevelBlocked;
        this.levelPrice = levelPrice;
        this.levelImage.sprite = levelImage;

        textLevelPrice.text = levelPrice.ToString();

        SetLevelIconText();
        UpdateLevelIconValues(levelHighScore);
    }

    public void UpdateLevelIconValues(int levelHighScore) {
        this.levelHighScore = levelHighScore;
        UpdateLevelIconText();
    }

    public void SetLevelIconText() {
        textLevelName.text = levelName;
    }

    public void UpdateLevelIconText() {
        textLevelScore.text = levelHighScore.ToString();
    }

    public void TryUnlockLevel()
    {
        if (GameManager.Instance.TryUnlockLevel(levelId))
            isBlocked = false;
        MainMenuUIManager.Instance.UpdateScoreText();
    }


}
