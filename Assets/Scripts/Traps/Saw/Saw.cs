using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Traps
{
    

    private Animator anim;

    [SerializeField] private float speed = 5;
    [SerializeField] private Transform[] checkPoint;
    [SerializeField] private float cooldown = 1;

    private int checkPointIndex;
    private float cooldownTimer;
 
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = checkPoint[0].position;
    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= Time.deltaTime;

        bool isWorking = cooldownTimer < 0;
        anim.SetBool("isWorking", isWorking);

        transform.position = Vector3.MoveTowards(transform.position, checkPoint[checkPointIndex].position,speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, checkPoint[checkPointIndex].position) < 0.15f)
        {
            Flip();
            cooldownTimer = cooldown;
            checkPointIndex++;
            if(checkPointIndex >= checkPoint.Length)
            {
                checkPointIndex = 0;
            }
        }

    }

    private void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }


}
