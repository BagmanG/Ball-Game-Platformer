using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public GameObject ExitButton;
    public GameObject LevelButton;
    public Transform LevelsRoot;

    [SerializeField] private Image AudioImage;
    [SerializeField] private Sprite AudioOn;
    [SerializeField] private Sprite AudioOff;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LoadPlatformFunctions();
        LoadLevelsData();
        AudioManager.PlayMusic(LevelAudioType.Forest);
        UpdateAudioImage();
    }

    private void UpdateAudioImage()
    {
        AudioImage.sprite = AudioListener.volume == 0 ? AudioOff : AudioOn;
    }

    public void MuteAudio()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
        UpdateAudioImage();
        PlayerPrefs.SetInt("Audio", (int)AudioListener.volume);
        PlayerPrefs.Save();
        EventSystem.current.SetSelectedGameObject(null);
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

    public void Play()
    {
        int maxLevel = 0;
        for (int i = 1; i <= 18; i++)
        {
            int level = PlayerPrefs.GetInt($"Level{i}", 0);
            if(level == 1)
            {
                maxLevel = i;
            }
        }
        if(maxLevel == 1 || maxLevel == 18)
        {
            SceneManager.LoadScene($"Level1");
            return;
        }
        SceneManager.LoadScene($"Level{maxLevel+1}");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
