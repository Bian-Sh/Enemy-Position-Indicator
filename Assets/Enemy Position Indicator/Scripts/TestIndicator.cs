using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestIndicator : MonoBehaviour
{
    public PositionIndicator indicator;
    public float duration = 2f;
    Vector3 leftCorner, rightCorner;
    private void Start()
    {
        leftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 50));
        rightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 50));
      //  indicator.SetTarget(transform);
     //    InvokeRepeating(nameof(UpdatePositon), 0.02f, duration);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
              indicator.SetTarget(transform);

        }
    }
    private void UpdatePositon()
    {
        var x = Random.Range(leftCorner.x, rightCorner.x);
        var y = Random.Range(leftCorner.y, rightCorner.y);
        var pos = new Vector3(x, y, leftCorner.z * 0.4f);
        transform.position = pos;
    }
}
