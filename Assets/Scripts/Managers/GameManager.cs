using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private Animator Animator;
    private BallController Player;

    private bool levelComplete = false;

    [SerializeField] private GameObject FinishUI;
    [SerializeField] private Text LevelTitle;
    [SerializeField] private GameObject PauseUI;
    private void Start()
    {
        levelComplete = false;
        Animator = GetComponent<Animator>();
        Player = FindFirstObjectByType<BallController>();
        LevelTitle.text = $"�������: {GetLevelIndex()}";
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
            //TODO
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
