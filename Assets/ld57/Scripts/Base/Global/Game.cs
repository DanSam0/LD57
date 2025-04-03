using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent (typeof(AudioLogic))]
[DefaultExecutionOrder(-9999)]
public class Game : MonoBehaviour
{

    public static Game Instance;

    public bool isPaused;
    public List<IPausable> pausableList = new List<IPausable>();
    public JsonPlayerPrefs Preferences { get; private set; }

    public AudioLogic audioLogic;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }

        Instance = this;
        DontDestroyOnLoad(this);

        audioLogic = GetComponent<AudioLogic>();

        Preferences = new JsonPlayerPrefs(Application.persistentDataPath + "/Preferences.json");
    }


    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if(pausableList.Count > 0)
        {
            foreach (IPausable pausable in pausableList)
            {
                pausable.OnPause();
            }
        }        
    }


    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausableList.Count > 0)
        {
            foreach (IPausable pausable in pausableList)
            {
                pausable.OnResume();
            }
        }
    }


    public void Exit()
    {
        Application.Quit();
    }


    private void OnApplicationQuit()
    {
        Preferences.Save();
    }


    public void ReloadScene()
    {
        Resume();
        LoadScene(SceneManager.GetActiveScene().name);
    }


    public void LoadScene(string sceneName)
    {
        Resume();
        SceneManager.LoadScene(sceneName);
    }

}
