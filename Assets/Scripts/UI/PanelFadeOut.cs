using System.Collections;
using UnityEngine;

public class PanelFadeOut : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Fade()
    {
        animator.Play("UiFadeOut");
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
