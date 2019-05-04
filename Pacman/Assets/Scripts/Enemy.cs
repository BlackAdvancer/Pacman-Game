using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Enemy : MonoBehaviour
{
    public float speed = 0.35f;
    public Transform startPoint;
    public Transform homePoint;
    private Vector2 dest = Vector2.zero;
    private direction preDirtion = direction.center;
    private List<direction> directionChoose;

    enum direction
    {
        center,
        up,
        down,
        left,
        right
    };


    private void Start()
    {
        transform.position = homePoint.position;
        dest = startPoint.position;
    }

    private void FixedUpdate()
    {
        moving();
    }

    private void moving()
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(temp);
        if ((Vector2)transform.position == dest)
        {
            direction next = NextDirection();
            if (next == direction.up)
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if (next == direction.down)
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if (next == direction.left)
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if (next == direction.right)
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
        }

        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("dirX", dir.x);
        GetComponent<Animator>().SetFloat("dirY", dir.y);

    }

    /**
     * 
     * This is enemy's AI system. It decides the next direction
     * when enemy needs to change its current direction.
     * This function creates eight lines from the enemy to its potential
     * next postion. Each direction(up, dowm, left, right) has tow parallel 
     * lines and the distance of this lines can fit the radius of enemy.
     * If the line is stoped by mase wall, then that direction will be
     * rejected. Finally, then enemy will choose availlable directions randomly
     * 
     * */
    private direction NextDirection()
    {
        directionChoose = new List<direction>();
        Vector2 pos = transform.position;
        System.Random random = new System.Random();
        int option = 0;

        RaycastHit2D hitRight1 = Physics2D.Linecast(new Vector2((float)(pos.x + 1.25), (float)(pos.y + 0.9)), new Vector2(pos.x,(float)(pos.y + 0.9)));
        RaycastHit2D hitRight2 = Physics2D.Linecast(new Vector2((float)(pos.x + 1.25), (float)(pos.y - 0.9)), new Vector2(pos.x, (float)(pos.y - 0.9)));
        RaycastHit2D hitLeft1 = Physics2D.Linecast(new Vector2((float)(pos.x - 1.25), (float)(pos.y + 0.9)), new Vector2(pos.x, (float)(pos.y + 0.9)));
        RaycastHit2D hitLeft2 = Physics2D.Linecast(new Vector2((float)(pos.x - 1.25), (float)(pos.y - 0.9)), new Vector2(pos.x, (float)(pos.y - 0.9)));
        RaycastHit2D hitUp1 = Physics2D.Linecast(new Vector2((float)(pos.x + 0.9), (float)(pos.y + 1.25)), new Vector2((float)(pos.x + 0.9), pos.y));
        RaycastHit2D hitUp2 = Physics2D.Linecast(new Vector2((float)(pos.x - 0.9), (float)(pos.y + 1.25)), new Vector2((float)(pos.x - 0.9), pos.y));
        RaycastHit2D hitDown1 = Physics2D.Linecast(new Vector2((float)(pos.x + 0.9), (float)(pos.y - 1.25)), new Vector2((float)(pos.x + 0.9), pos.y));
        RaycastHit2D hitDown2 = Physics2D.Linecast(new Vector2((float)(pos.x - 0.9), (float)(pos.y - 1.25)), new Vector2((float)(pos.x - 0.9), pos.y));

        if ((hitRight1.collider.gameObject.name != "Maze") && (hitRight2.collider.gameObject.name != "Maze") && preDirtion != direction.left)
        {
            directionChoose.Add(direction.right);
            option++;
        }
        if ((hitLeft1.collider.gameObject.name != "Maze") && (hitLeft2.collider.gameObject.name != "Maze") && preDirtion != direction.right) {
            directionChoose.Add(direction.left);
            option++;
        }
            
        if ((hitUp1.collider.gameObject.name != "Maze") && (hitUp2.collider.gameObject.name != "Maze") && preDirtion != direction.down)
        {
            directionChoose.Add(direction.up);
            option++;
        }
        if ((hitDown1.collider.gameObject.name != "Maze") && (hitDown2.collider.gameObject.name != "Maze") && preDirtion != direction.up)
        {
            directionChoose.Add(direction.down);
            option++;
        }
        int nextdirection = random.Next(option);
        preDirtion = directionChoose[nextdirection];
        return directionChoose[nextdirection];

    }

    /*
     * eat the Pacman
     * 
     * */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.pacman.GetComponent<Pacman>().isSuperPacman)
            {
                GameManager.Instance.EatEmeny(); 
                transform.position = homePoint.position;
                dest = startPoint.position;
            }
            else{
                collision.gameObject.SetActive(false);
            }
            
        }
    }
}
