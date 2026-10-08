using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    private Vector3 offset;

    void Start()
    {
        offset = target.position - transform.position;
    }

    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        transform.position = target.position - offset;
    }
}