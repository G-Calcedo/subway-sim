using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPivot : MonoBehaviour
{
    private bool locked;
    private Camera camera;
    private Vector3[] angles = new Vector3[5];
    private Vector3[] positions = new Vector3[5];
    private float[] FOV = new float[5];
    private int cont;

    private void Awake()
    {
        cont = 0;
        camera = GetComponentInChildren<Camera>();
      
        positions[0] = new Vector3(0, 0, 0);
        positions[1] = new Vector3(-3.3f, -12.3f, -75f);
        positions[2] = new Vector3(-9f, 0.7f, 2.2f);
        positions[3] = new Vector3(53f, -15.7f, 1.25f);
        positions[4] = new Vector3(-53.5f, -36f, 49f);

        angles[0] = new Vector3(0, 0, 0);
        angles[1] = new Vector3(-18f, -27f, -19f);
        angles[2] = new Vector3(-16f, -13.2f, -15.65f);
        angles[3] = new Vector3(-15.5f, 85f, -16f);
        angles[4] = new Vector3(-3.5f, 45.85f, -2.75f);

        FOV[0] = 60f;
        FOV[1] = 15f;
        FOV[2] = 10f;
        FOV[3] = 22f;
        FOV[4] = 55f;
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
