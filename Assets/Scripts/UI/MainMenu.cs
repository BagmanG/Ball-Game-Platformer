using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public GameObject ExitButton;
    public GameObject LevelButton;
    public Transform LevelsRoot;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LoadPlatformFunctions();
        LoadLevelsData();
    }
    private void LoadLevelsData()
    {
        for(int i = 1; i <= 18; i++)
        {
            GameObject levelButton = Instantiate(LevelButton);
            levelButton.transform.SetParent(LevelsRoot.transform, false);
            levelButton.GetComponent<LevelButtonHandler>().Init(i);
        }
    }

    private void LoadPlatformFunctions()
    {
        switch (GlobalVars.Platform)
        {
            case Platform.PC:
                {
                    ExitButton.SetActive(true);
                    break;
                }
        }
    }

    public void TestLoad()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
