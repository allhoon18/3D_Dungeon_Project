using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform targetTransform;

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Start()
    {
        startPos = transform.position;
        targetPos = targetTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != targetPos)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1f * Time.deltaTime);
        else
        {
            if (targetPos == targetTransform.position)
                targetPos = startPos;
            else if (targetPos == startPos)
                targetPos = targetTransform.position;
        }
        
    }
}
