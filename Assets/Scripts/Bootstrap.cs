using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    public Platform Platform;
    private void Awake()
    {
        GlobalVars.Platform = Platform;

        PlayerPrefs.SetInt("Level1",1);
        PlayerPrefs.Save();

        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
