using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Traps : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null){

            Player player = collision.GetComponent<Player>();

            player.KnockBack(transform);

        }
    }
}
