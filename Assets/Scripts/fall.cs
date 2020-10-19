using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player1")
        {
            permanentUI.perm.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

