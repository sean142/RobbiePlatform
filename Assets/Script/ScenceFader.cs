using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceFader : MonoBehaviour
{
    Animator anim;
    int faderID;

    private void Start()
    {
        anim = GetComponent<Animator>();

        faderID = Animator.StringToHash("Fade");

        GameManager.RegisterScenceFader(this);
    }

    public void FadeOut()
    {
        anim.SetTrigger(faderID);
    }
}
