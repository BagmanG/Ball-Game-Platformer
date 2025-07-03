using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private Animator Animator;
    private BallController Player;

    private bool levelComplete = false;

    [SerializeField] private GameObject FinishUI;
    private void Start()
    {
        levelComplete = false;
        Animator = GetComponent<Animator>();
        Player = FindFirstObjectByType<BallController>();
    }

    private void Update()
    {
        HandleReloadLevel();
    }

    private void HandleReloadLevel()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
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
