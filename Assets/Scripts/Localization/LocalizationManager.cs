using Assets.Scripts.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    private void Start()
    {
        Application.ExternalCall("Inited");
    }
    public void SelectLang(int langID)
    {
        Lang.CurrentLang = langID;
        SceneManager.LoadScene("InitScene");
    }
}
