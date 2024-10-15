using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBait : MonoBehaviour
{
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
    }
}
