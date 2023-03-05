using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceFader : MonoBehaviour
{
    Animator animator;
    int faderID;

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    private void Start()
    {
        faderID = Animator.StringToHash("Fade");

        GameManager.RegisterScenceFader(this);
    }

    public void FadeOut()
    {
        animator.SetTrigger(faderID);
    }
}
