using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - Player.transform.position;
    }

    // LateUpdate is called once per frame and is guaranteed to run after all items have been processed in Update
    void LateUpdate()
    {
        transform.position = Player.transform.position + offset;
    }
}
