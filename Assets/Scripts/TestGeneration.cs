using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    public GameObject prefabHexagon;
    public float radius = 10.0f;
    public float blockRadius = 1.0f;
    public int pointsCount = 100;
    public List<GameObject> objects = new List<GameObject>();
    public List<Vector3> points = new List<Vector3>();
     
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            DestroyBlocks();
            CreateBlocks();
        } 
    }

    void CreateBlocks ()
    {
        Vector3 planetCenter = Vector3.zero;

        float nCount = 0;
        float a = 4 * Mathf.PI * radius * radius / (float)pointsCount;
        float d = Mathf.Sqrt(a);
        float Mtheta = Mathf.Round(Mathf.PI / d);
        float dTheta = Mathf.PI / Mtheta;
        float dPhi = a / dTheta;

        Debug.Log(Mtheta);
        for (int i = 0; i < Mtheta; i++)
        {
            float theta = Mathf.PI * (i + 0.5f) / Mtheta;
            Debug.Log("theta: " + theta);
            float MPhi = Mathf.Round(2 * Mathf.PI * Mathf.Sin(theta) / dPhi);
            Debug.Log("MPhi: " + MPhi);
            for (int j = 0; j < MPhi; j++)
            {
                float phi = 2 * Mathf.PI * j / MPhi;

                Vector3 point = new Vector3(
                    Mathf.Sin(theta) * Mathf.Cos(phi),
                    Mathf.Sin(theta) * Mathf.Sin(phi),
                    Mathf.Cos(theta)
                ) * radius;
                points.Add(point);


                var t = Instantiate(prefabHexagon).transform;
                t.position = point;
                t.up = point - planetCenter;
                objects.Add(t.gameObject);
                nCount++;
            }
        }
    }

    void DestroyBlocks ()
    {
        points.Clear();
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
        objects.Clear();
    }

    void OnDrawGizmos ()
    {
        foreach (var point in points)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
       
    }
}
