using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditorInternal;


[CreateAssetMenu(fileName = "Some Scene Reference", menuName = "Scriptable Objects/Scene Management/Scene Reference", order = 1)]
public class SceneReference : ScriptableObject
{
#if UNITY_EDITOR
    public SceneAsset levelScene;
#endif

    [SerializeField] protected string _levelName;
    [SerializeField] protected int _levelId;


    public string levelName {
        get { return _levelName; }
    }
    public int levelId {
        get { return _levelId; }
    }

    public Sprite levelSprite;
    public bool isLevelBlocked;
    public int levelPrice;
    public int highScore;

    public string levelPath;

    Action LoadLevelAction;

    public void UpdateActions()
    {
        LoadLevelAction = () => SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Single);
    }

    public void ReloadLevel(FadeScreen screenFader)
    {
        if (LoadLevelAction == null)
            UpdateActions();

        screenFader.FadeOut(LoadLevelAction);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Single);
    }
}

[CustomEditor(typeof(SceneReference))]
public class SceneReferenceEditor : Editor
{
    private SceneReference sceneReference;

    public SerializedProperty levelSceneProperty;

    public SerializedProperty levelNameProperty;
    public SerializedProperty levelIdProperty;
    public SerializedProperty isLevelBlockedProperty;
    public SerializedProperty levelPriceProperty;
    public SerializedProperty highScoreProperty;
    public SerializedProperty levelSpriteProperty;

    public SerializedProperty levelPathProperty;

    private void OnEnable()
    {
        Init();
    }

    public override void OnInspectorGUI()
    {
        sceneReference = (SceneReference)target;
        serializedObject.Update();

        sceneReference.levelPath = AssetDatabase.GetAssetPath(sceneReference.levelScene);

        DrawProperties();

        serializedObject.ApplyModifiedProperties();
    }

    public virtual void Init()
    {
        levelSceneProperty = serializedObject.FindProperty("levelScene");

        levelNameProperty = serializedObject.FindProperty("_levelName");
        levelIdProperty = serializedObject.FindProperty("_levelId");
        isLevelBlockedProperty = serializedObject.FindProperty("isLevelBlocked");
        levelPriceProperty = serializedObject.FindProperty("levelPrice");
        highScoreProperty = serializedObject.FindProperty("highScore");
        levelSpriteProperty = serializedObject.FindProperty("levelSprite");

        levelPathProperty = serializedObject.FindProperty("levelPath");
    }

    public virtual void DrawProperties()
    {
        EditorGUILayout.LabelField("Scene Reference", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelSceneProperty);

        EditorGUILayout.LabelField("Level Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelNameProperty);
        EditorGUILayout.PropertyField(levelIdProperty);
        EditorGUILayout.PropertyField(isLevelBlockedProperty);

        EditorGUILayout.PropertyField(levelPriceProperty);
        EditorGUILayout.PropertyField(highScoreProperty);

        EditorGUILayout.PropertyField(levelSpriteProperty);

        EditorGUILayout.LabelField("Scene Path Index [Don't touch]", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelPathProperty);
    }
}

