using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Core;

public class DrawInputAreasDebug : DeskaBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void OnDrawGizmos()
    {
        

        Vector2 size = new Vector2(Camera.main.orthographicSize , Camera.main.orthographicSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.75f, 0f)), size);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.25f, 0f)), size);
    }
}
