using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5.0f;
    [SerializeField] private float targetedSpeed = 7.0f;
    [SerializeField] private GameObject fishBait;
    private Vector3 direction;
    private float speed;
    private bool isChasingBait = false;

    private void Start()
    {
        direction = Vector3.right;
        speed = normalSpeed;
    }

    private void Update()
    {
        Vector3 fishBaitPos = fishBait.transform.localPosition;
        if (fishBaitPos.y < -0.5f)
        {
            direction = (fishBait.transform.position - transform.position).normalized;
            speed = targetedSpeed;
            isChasingBait = true;
        }
        else if (isChasingBait)
        {
            isChasingBait = false;

            if (direction.x > 0)
            {
                direction = Vector3.right;
            }
            else
            {
                direction = Vector3.left;
            }
            speed = normalSpeed;
        }

        if (!isChasingBait)
        {
            if (transform.position.x > 8)
            {
                direction = Vector3.left;
            }
            else if (transform.position.x < -8)
            {
                direction = Vector3.right;
            }
        }

        transform.position += direction * speed * Time.deltaTime;
    }
}
