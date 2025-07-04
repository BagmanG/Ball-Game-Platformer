using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    public Text Text;
    private Color lockedLevel = new Color(230,230,230);
    private Color unlockedLevel = Color.yellow;
    private bool unlocked = false;
    private int index;
    public void Init(int index)
    {
        this.index = index;
        Text.text = index.ToString();
        unlocked =  PlayerPrefs.GetInt($"Level{index}",0) == 1;
        Text.color = unlocked ? unlockedLevel : lockedLevel;
    }

    public void StartLevel()
    {
        if (unlocked)
        {
            SceneManager.LoadScene($"Level{this.index}");
        }
    }
}
