using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBlock : MonoBehaviour 
{
 private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player1")
        {
            collision.gameObject.GetComponent<player4>().enabled = false;
        }
    }
}
