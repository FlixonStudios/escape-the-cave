using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip ("Game units per second")]
    [SerializeField] float verticalScrollUpSpeed = 1.0f;
    

    void Update()
    {
        float yMove = verticalScrollUpSpeed * Time.deltaTime;
        transform.Translate(new Vector2(0f, yMove));                
    }

}
