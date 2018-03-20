using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
    public GameObject reticle;

    public Color inactiveReticleColor = Color.gray;

    public Color activeReticleColor = Color.green;

    private GazableObject currentGazeObject;

    private GazableObject currentSelectedObject;
    private RaycastHit lastHit;

    // Use this for initialization
    void Start()
    {
        SetReticleColor(inactiveReticleColor);
    }

    // Update is called once per frame
    void Update()
    {

        ProcessGaze();

    }

    public void ProcessGaze()
    {
        Ray raycastRay = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawRay(raycastRay.origin, raycastRay.direction * 100);
        if(Physics.Raycast(raycastRay,out hitInfo))
        {
            Debug.Log("Hitting"+hitInfo.collider.gameObject.name);

            GameObject hitObj = hitInfo.collider.gameObject;

            GazableObject gazeObj = hitObj.GetComponentInParent<GazableObject>();
        }
        if (Physics.Raycast(raycastRay, out hitInfo))
        {
            GameObject hitObj = hitInfo.collider.gameObject;

            GazableObject gazeObj = hitObj.GetComponent<GazableObject>();

            if (gazeObj != null)
            {

                if (gazeObj != currentGazeObject)
                {
                    ClearCurrentObject();
                    currentGazeObject = gazeObj;
                    currentGazeObject = OnGazeEnter(hitInfo);
                    SetReticleColor(activeReticleColor);
                }
                else
                {
                    currentGazeObject.OnGaze(hitInfo);
                }
            }
            else
            {
                ClearCurrentObject();
            }
            lastHit = hitInfo;
        }
        else
        {
            ClearCurrentObject();
        }
            
        }
    

    private void SetReticleColor(Color reticleColor)
    {
        reticle.GetComponent<Renderer>().material.SetColor("Color",reticleColor);
    }

    private void CheckForInput(RaycastHit hitInfo)
    {
        if(Input.GetMouseButtonDown(0) && currentGazeObject != null)
        {
            currentSelectedObject = currentGazeObject;
            currentSelectedObject.OnPress(hitInfo);
        }
        if (Input.GetMouseButton(0) && currentGazeObject != null)
        {
            currentSelectedObject.OnHold(hitInfo);
        }
        if (Input.GetMouseButtonUp(0) && currentGazeObject != null)
        {
            currentSelectedObject.OnRelease(hitInfo);
            currentSelectedObject = null;
        }
    }

    private void ClearCurrentObject()
    {
        if(currentGazeObject != null)
        {
            currentGazeObject.OnGazeExit();
            SetReticleColor(inactiveReticleColor);
            currentGazeObject = null;
        }
    }
}
