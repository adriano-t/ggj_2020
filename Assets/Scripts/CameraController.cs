using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform following;
	public bool cameraBehind = false;

    [Header("Settings")]
    public Vector3 offset;
    public float rigidityPos, rigidityRot;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, following.position + following.up * offset.y + following.forward * offset.z, Time.deltaTime * rigidityPos);

        Vector3 dir = (following.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, following.up), Time.deltaTime * rigidityRot);

        
    }
}
