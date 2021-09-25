using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignOnEnable : MonoBehaviour
{
    public int attempts = 0;
    private int MaxAttempts = 20;
  
    void OnEnable()
    {       
        TryToFloor();      
    }

    private void TryToFloor()
    {
        float[] testArr = new float[] { 500, 500 };

        int testcount = 0;
        foreach (Transform child in transform)
        {
            // this is what gives us an exception. How to move them more cleverly?

            child.localPosition = new Vector3(child.position.x, GetTerrainPos(child.position.x, child.position.z).y, child.position.z);
            testArr[testcount] = child.position.y;
            testcount += 1;
        }


        if (testArr[0] == 0 && testArr[1] == 0)
        {
            if (attempts < MaxAttempts)
            {
                attempts += 1;
                Invoke(nameof(TryToFloor), 1);
                return;
            }
            else
            {
              
                Debug.LogFormat("Failed to floor splines {0} on countout", gameObject.name);
                return;
            }
        }       
    }

    static Vector3 GetTerrainPos(float x, float y)
    {
        
        //Create origin for raycast that is above the terrain. I chose 500.
        Vector3 origin = new Vector3(x, 500f, y);
      
        LayerMask mask = LayerMask.GetMask("World");

        Ray ray = new Ray(origin, Vector3.down);

        Physics.Raycast(ray, out RaycastHit hit, 501f, mask);

        //  Debug.Log("Terrain location found at " + hit.point);
        return hit.point;
    }
}
