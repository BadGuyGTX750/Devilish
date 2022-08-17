using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float distance;
    public GameObject player;

    public float followSpeed;
    public Vector2 posOffset;

    // Start is called before the first frame update
    void Start()
    {
        distance = -30;
        GameObject player = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 curPos = transform.position;

        Vector3 endPos = player.transform.position;
        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -30;

        transform.position = Vector3.Lerp(curPos , endPos , followSpeed * Time.deltaTime);

    }
}
