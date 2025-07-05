using Assets.Scripts.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    public void SelectLang(int langID)
    {
        Lang.CurrentLang = langID;
        SceneManager.LoadScene("InitScene");
    }
}
