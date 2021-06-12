using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody;
    BoxCollider2D myFeet;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }     
    }


    public bool IsFacingRight()
    {
        return (transform.localScale.x > 0);
    }
    public void FlipSpriteHorizontally()
    {        
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
    }
    public void FlipDirectionHozontally()
    {
        myRigidBody.velocity = -myRigidBody.velocity;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipDirectionHozontally();
        FlipSpriteHorizontally();
    }

}
