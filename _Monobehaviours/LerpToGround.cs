using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToGround : MonoBehaviour
{
    private Vector3 positionToMoveTo;
    public float time = 2f;


    void OnEnable()
    {


      Vector3 positionToMoveTo =  GetTerrainPos(transform.position.x, transform.position.z) + new Vector3(0, -1f, 0);


        if (positionToMoveTo.y > 0)
        {
            StartCoroutine(LerpPosition(positionToMoveTo, time));
        }

       


    }


    static Vector3 GetTerrainPos(float x, float y)  // The actual terrain. Ignoring Objects.
    {

        //Create object to store raycast data

        //Create origin for raycast that is above the terrain. I chose 500.
        Vector3 origin = new Vector3(x, 500f, y);

        //Send the raycast.
        // Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 501f);

        LayerMask mask = LayerMask.GetMask("World");

        Ray ray = new Ray(origin, Vector3.down);


        Physics.Raycast(ray, out RaycastHit hit, 501f, mask);


        //  Debug.Log("Terrain location found at " + hit.point);
        return hit.point;

    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}