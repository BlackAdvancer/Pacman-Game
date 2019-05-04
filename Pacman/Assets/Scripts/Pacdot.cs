using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour
{
    public bool isSuperdot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Pacman")
        {
            if (isSuperdot)
            {
                GameManager.Instance.EatSuperDot();
            }

            GameManager.Instance.Eatdot(gameObject);
            Destroy(gameObject);
        }
    }

}
