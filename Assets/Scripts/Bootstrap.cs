using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    public Platform Platform;
    private int CurrentVolume = 0;
    private void Awake()
    {
        GlobalVars.Platform = Platform;

        PlayerPrefs.SetInt("Level1",1);
        PlayerPrefs.Save();

        AudioListener.volume = PlayerPrefs.GetInt("Audio",1);

        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void Mute()
    {
        CurrentVolume = (int)AudioListener.volume;
        AudioListener.volume = 0;
    }

    public void UnMute()
    {
        AudioListener.volume = CurrentVolume;
    }
}
