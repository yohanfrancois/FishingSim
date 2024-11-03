using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBait : MonoBehaviour
{
    [SerializeField] private float waterSpeed = 0.5f;


    private LineRenderer lr;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isInWater = false;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, -waterSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            rb.gravityScale = 1.0f;
        }
    }
}
