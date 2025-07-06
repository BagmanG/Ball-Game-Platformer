using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Managers;
using UnityEngine.EventSystems;
using Assets.Scripts.Localization;
public class GameManager : MonoBehaviour
{
    private Animator Animator;
    private BallController Player;
    private bool levelComplete = false;

    private int currentLevel = 0;

    [SerializeField] private GameObject FinishUI;
    [SerializeField] private GameObject SkipLevelButton;
    [SerializeField] private Text LevelTitle;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private Image AudioImage;
    [SerializeField] private Sprite AudioOn;
    [SerializeField] private Sprite AudioOff;
    private void Start()
    {
        levelComplete = false;
        Animator = GetComponent<Animator>();
        Player = FindFirstObjectByType<BallController>();
        currentLevel = GetLevelIndex();
        LevelTitle.text = $"{Lang.Get(7)}: {currentLevel}";

        InitAudio();
        UpdateAudioImage();
        SkipLevelButton.SetActive(GlobalVars.Platform == Platform.WebDesktop);
    }

    public void SkipLevel()
    {
        Application.ExternalCall("SkipLevel");
    }

    public void OnSkipLevel()
    {
        levelComplete = true;
        OnFadeAnimationEnd();
    }

    private void UpdateAudioImage()
    {
        AudioImage.sprite = AudioListener.volume == 0 ? AudioOff : AudioOn;
    }

    public void MuteAudio()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
        UpdateAudioImage();
        PlayerPrefs.SetInt("Audio",(int)AudioListener.volume);
        PlayerPrefs.Save();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetAudioVolume(int value)
    {
        AudioListener.volume = value;
    }

    private void InitAudio()
    {
        AudioManager.PlayMusic(currentLevel >= 11 ? LevelAudioType.Cave : LevelAudioType.Forest);
        if (currentLevel >= 11)
        {
            AudioReverbFilter audioReverbFilter = FindFirstObjectByType<CameraFollow>().gameObject.AddComponent<AudioReverbFilter>();
            audioReverbFilter.reverbPreset = AudioReverbPreset.Cave;
        }
    }

    private int GetLevelIndex()
    {
        return int.Parse(SceneManager.GetActiveScene().name.Substring("Level".Length));
    }

    private void Update()
    {
        HandleReloadLevel();
        HandlePause();
    }

    private void HandleReloadLevel()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.SetActive(!PauseUI.activeSelf);
            Player.CanMove = !PauseUI.activeSelf;
        }
    }

    public void Continue()
    {
        Player.CanMove = true;
        PauseUI.SetActive(false);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void ReloadLevel()
    {
        Animator.Play("FadeOutScreen");
    }

    public void OnFadeAnimationEnd()
    {
        if (levelComplete == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            PlayerPrefs.SetInt($"Level{currentLevel}",1);
            PlayerPrefs.Save();
            if(currentLevel == 18)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene($"Level{currentLevel + 1}");
            }
        }
    }

    public void OnFinish()
    {
        levelComplete = true;
        Player.Freeze();
        FinishUI.SetActive(true);
        StartCoroutine(DelayFade(1));
    }

    private IEnumerator DelayFade(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Animator.Play("FadeOutScreen");
    }
}
