using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawExtended : Traps
{
    private Animator anim;

    [SerializeField] private float speed = 5;
    [SerializeField] private Transform[] checkPoint;

    private int checkPointIndex;
    private bool goFoward = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);
        transform.position = checkPoint[0].position;
        Flip();
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = Vector3.MoveTowards(transform.position, checkPoint[checkPointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, checkPoint[checkPointIndex].position) < 0.15f)
        {

            if(checkPointIndex == 0)
            {
                Flip();
                goFoward = true;
            }

            if (goFoward)
            {
                checkPointIndex++;
            }
            else
            {
                checkPointIndex--;
            }

            if (checkPointIndex >= checkPoint.Length)
            {
                checkPointIndex = checkPoint.Length - 1;
                goFoward = false;
                Flip();
            }
        }

    }

    private void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }

}
