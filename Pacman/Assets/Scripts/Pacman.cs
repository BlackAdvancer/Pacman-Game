using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public float speed = 0.35f;
    public bool isSuperPacman = false;
    private Vector2 dest = Vector2.zero;

    // Use this for initialization
    private void Start()
    {
        dest = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moving();
    }

    /*
     *Decide Pacman moving direction by KeyCode and set the animator
     * 
     * */
    private void moving()
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(temp);
        if ((Vector2)transform.position == dest)
        {
            if (Input.GetKey(KeyCode.W) && IsVaild(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if (Input.GetKey(KeyCode.S) && IsVaild(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if (Input.GetKey(KeyCode.A) && IsVaild(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if (Input.GetKey(KeyCode.D) && IsVaild(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
        }

        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("dirX", dir.x);
        GetComponent<Animator>().SetFloat("dirY", dir.y);
    }

    /*
     * check if the Pacman can still move towards a certain direction
     * 
     * */
    private bool IsVaild(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }
}
