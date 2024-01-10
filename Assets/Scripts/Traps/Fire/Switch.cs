using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Fire myTraps;
    private Animator anim;

  
    [SerializeField] private float timeNotActive = 2;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            

            anim.SetTrigger("pressed");
            myTraps.FireSwitchAfterPress(timeNotActive);
        }
    }

}
