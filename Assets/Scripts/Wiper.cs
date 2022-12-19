using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiper : MonoBehaviour
{
    public Animator animator;
    public float speed;

    public KeyCode wiperKey = KeyCode.I;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Wiper", 1);
        if (animator != null)
        {
            if (Input.GetKeyDown(wiperKey))
            {
                animator.SetFloat("Wiper", 1);
                animator.speed = speed;
            }
        }
    }
}
