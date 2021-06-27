using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPivot : MonoBehaviour
{
    private bool locked;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !locked)
        {
            locked = true;
            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -90, 0), 0.75f)
                .OnComplete(() => locked = false);
        }

        if (Input.GetKeyDown(KeyCode.A) && !locked)
        {
            locked = true;
            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 90, 0), 0.75f)
                .OnComplete(() => locked = false);
        }
    }
}
