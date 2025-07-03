using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
