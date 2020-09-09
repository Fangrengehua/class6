using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObject : MonoBehaviour
{
    //锚定了屏幕的中心最佳位置
    public Transform CenterTransform;

    // Start is called before the first frame update
    void Start()
    {
        CenterTransform = GameObject.Find("CenterAnchor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            transform.position = Vector3.Lerp(transform.position, CenterTransform.position, 3.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, CenterTransform.rotation, 3.5f);
        }
    }

    bool move = false;
    public void MoveToCenter()
    {
        move = true;
    }

    public void ResetMove()
    {
        move = false;
    }
}
