using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance {  get; private set; }

    private int _currentScene;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void ChangeScene(int pScene)
    {
        _currentScene = pScene;
        SceneManager.LoadScene(pScene);
    }

    public void ChangeScene(string pScene)
    {
        _currentScene = SceneManager.GetSceneByName(pScene).buildIndex;
        SceneManager.LoadScene(pScene);
    }

    public void NextScene()
    {
        _currentScene += 1;
        SceneManager.LoadScene(_currentScene);
    }

    public void PreviousScene()
    {
        _currentScene -= 1;
        SceneManager.LoadScene(_currentScene);
    }

    public int CurrentScene
    { get { return _currentScene; } }
}
