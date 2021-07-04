using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPivot : MonoBehaviour
{
    private bool locked;
    private Camera camera;
    private Vector3[] angles = new Vector3[4];
    private Vector3[] positions = new Vector3[4];
    private float[] FOV = new float[4];
    private int cont;

    private void Awake()
    {
        cont = 0;
        camera = GetComponentInChildren<Camera>();
      
        positions[0] = new Vector3(0, 0, 0);
        positions[1] = new Vector3(5.67f, -10.71f, -77.47f);
        positions[2] = new Vector3(-11.3f, -0.25f, 1.55f);
        positions[3] = new Vector3(37.5f, -9.2f, 1.55f);

        angles[0] = new Vector3(0, 0, 0);
        angles[1] = new Vector3(-15.395f, -29.125f, -18.25f);
        angles[2] = new Vector3(-16.7f, -11.343f, -14.666f);
        angles[3] = new Vector3(-16.7f, 91f, -13.653f);

        FOV[0] = 60f;
        FOV[1] = 15f;
        FOV[2] = 7.5f;
        FOV[3] = 26f;
    }

    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow)) && !locked)
        {
            locked = true;

            CheckerCameraLeft();
            transform.DORotate(angles[cont], 0.75f).OnComplete(() => locked = false);
            transform.DOMove(positions[cont], 0.75f).OnComplete(() => locked = false);
            camera.fieldOfView = FOV[cont];

        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow)) && !locked)
        {
            locked = true;

            CheckerCameraRight();

            transform.DORotate(angles[cont], 0.75f).OnComplete(() => locked = false);
            transform.DOMove(positions[cont], 0.75f).OnComplete(() => locked = false);
            camera.fieldOfView = FOV[cont];
        }

       



    }

    public void CheckerCameraRight()
    {
        if (cont == 0)
        {
            cont = (angles.Length);
        }
        cont--;

        if (cont == 0)
        {
            camera.orthographic = true;
        }
        else
        {
            camera.orthographic = false;
            camera.orthographic = false;
        }

       
    }

    public void CheckerCameraLeft()
    {
        if (cont == (angles.Length - 1))
        {
            cont = -1;
        }
        cont++;
        if (cont == 0)
        {
            camera.orthographic = true;
        }
        else
        {
            camera.orthographic = false;

        }
    }
}
