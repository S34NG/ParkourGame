using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Traps
{

    public bool isWorking;

    private Animator animator;
    
    public float repeatRate;


    private void Start()
    {
        animator = GetComponent<Animator>();

        

        if (transform.parent == null)
        {
            InvokeRepeating("FireSwitch", 0, repeatRate);
        }

    }

    private void Update()
    {
        animator.SetBool("isWorking", isWorking);
    }

    public void FireSwitch()
    {
        isWorking = !isWorking;
    }

    public void FireSwitchAfterPress(float seconds)
    {
        CancelInvoke();
        isWorking = false;
        Invoke("FireSwitch", seconds);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(isWorking)
        {
            base.OnTriggerEnter2D(collision);
        }
    }
}
