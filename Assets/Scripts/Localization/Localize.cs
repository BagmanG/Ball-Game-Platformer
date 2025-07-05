using Assets.Scripts.Localization;
using UnityEngine;
using UnityEngine.UI;
public enum LocalizeType
{
    Text,Image
}
public class Localize : MonoBehaviour
{
    [SerializeField] private LocalizeType Type;
    [SerializeField] private Material Ru, En;
    [SerializeField] private int Key;

    private void Start()
    {
       if(Type == LocalizeType.Text)
        {
            GetComponent<Text>().text = Lang.Get(Key);
        }
        else
        {
            GetComponent<MeshRenderer>().material = Lang.CurrentLang == 0 ? Ru : En;
        }
    }
}
