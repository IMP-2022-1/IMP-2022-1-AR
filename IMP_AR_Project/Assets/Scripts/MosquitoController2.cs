using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController2 : MosquitoController
{

    public override void MosquitoMoving()
    {
        if (MosquitoMovingChoice == 0) // Moving 0
        {
            transform.RotateAround(RI.CenterOfRotation, RI.RotatingDirection, Time.deltaTime * transform.localScale.magnitude * 400);
        }
        else if (MosquitoMovingChoice == 1) // Moving 1
        {
            if (RightLeft)
            {
                //Debug.Log("right");
                transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 60);
                if ((OriPosition - transform.position).magnitude > 0.3)
                    RightLeft = false;
            }
            else
            {
                //Debug.Log("Left");
                transform.RotateAround(Player.transform.position, -Vector3.up, Time.deltaTime * transform.localScale.magnitude * 60);
                if ((OriPosition - transform.position).magnitude < 0.01)
                    RightLeft = true;
            }
        }
        else if (MosquitoMovingChoice == 2) // Moving 2
        {
            transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 60);
        }
        else if (MosquitoMovingChoice == 3) // Moving 3
        {
            float MagnitudeOfScale = transform.localScale.magnitude;
            transform.RotateAround(OriPosition - transform.localScale / 3, Vector3.back, Time.deltaTime * MagnitudeOfScale * 1000);
        }
    }
}
