using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WiperState
{
    Off,
    Low,
    Medium,
    High
}
public class Wiper : MonoBehaviour
{
    public Animator animator;
    public WiperState current = WiperState.Off;
    public KeyCode wiperKey = KeyCode.I;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            if (Input.GetKeyDown(wiperKey))
            {
                StopCoroutine(WiperCorutine());
;                StartCoroutine(WiperCorutine());
            }
        }
    }

    IEnumerator WiperCorutine()
    {
        float i = 0;
        if (current == WiperState.Off)
        {
            i = 0;
            animator.Play("Wiper", 1, 0f);
            while (i <= 1)
            {
                animator.SetFloat("Wiper", i);
                i += 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
            animator.SetFloat("Wiper", 1);
            current++;
        }
        else if(current == WiperState.High)
        {
            i = 1;
            while(i >= 0)
            {
                animator.SetFloat("Wiper", i);
                i -= 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
            animator.SetFloat("Wiper", 0);
            current = WiperState.Off;
        }
        else
        {
            current++;
            animator.speed = (int)current;
        }
        
    }
}
