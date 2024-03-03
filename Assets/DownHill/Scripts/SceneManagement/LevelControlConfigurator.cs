using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Level Control Configurator", menuName = "Scriptable Objects/Scene Management/Level Control Configurator", order = 1)]
public class LevelControlConfigurator : ScriptableObject
{
    [System.Serializable]
    public struct SceneSets {
        public string sceneName;
        public SceneReference sceneReference;
    }

    public List<SceneSets> sceneSets = new List<SceneSets>();


    public void LoadSceneName(string loadSceneName, FadeScreen fadeScreen)
    {
        for (int i = 0; i < sceneSets.Count; i++)
        {
            if (sceneSets[i].sceneName == loadSceneName)
            {
                //                Debug.Log("LoadSceneName: "+ loadSceneName);
            sceneSets[i].sceneReference.ReloadLevel(fadeScreen);
            }
        }
    }

    public void LoadScenePath(string loadScenePath , FadeScreen fadeScreen)
    {
        for (int i = 0; i < sceneSets.Count; i++)
        {
            if (sceneSets[i].sceneReference.levelPath == loadScenePath)
            {
                //                Debug.Log("LoadScenePath: "+ loadScenePath);
            sceneSets[i].sceneReference.ReloadLevel(fadeScreen);
            }
        }
    }

    public void LoadLevelByIndex(int levelId, FadeScreen fadeScreen)
    {
        for (int i = 0; i < sceneSets.Count; i++)
        {
            SceneReference lvlSceneRef = sceneSets[i].sceneReference;
            if (lvlSceneRef.levelId == levelId)
            {
                //Debug.Log("LoadScenePath: " + levelId);
                sceneSets[i].sceneReference.ReloadLevel(fadeScreen);
            }
        }
    }

    public int GetLevelCount()
    {
        int count = 0;
        for (int i = 0; i < sceneSets.Count; i++)
        {
            count++;
        }
        return count;
    }

    public string GetLevelName(int levelInd)
    {
        for (int i = 0; i < sceneSets.Count; i++)
        {
            SceneReference lvlSceneRef = sceneSets[i].sceneReference;
            if (lvlSceneRef.levelId == levelInd)
                return lvlSceneRef.levelName;
        }
        Debug.Log("There is no level Number " + levelInd);
        return null;
    }

    public bool GetLevelIsCompleted(int levelInd)
    {
        for (int i = 0; i < sceneSets.Count; i++)
        {
            SceneReference lvlSceneRef = sceneSets[i].sceneReference;
            if (lvlSceneRef.levelId == levelInd)
                return !lvlSceneRef.isLevelBlocked;
        }
        Debug.Log("There is no level Number " + levelInd);
        return false;
    }
}
