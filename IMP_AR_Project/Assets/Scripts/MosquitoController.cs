using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    public GameObject Player;

    // Field of Mosquito
    private int MosquitoMovingChoice;
    private AudioSource MqAudioSource;

    // Accoding to kind of Mosquito, HP & Damage is different 
    public int MosquitoHP = 1;
    public int MosquitoDamage = 1;

    // Used in Moving
    private Vector3 OriPosition;

    // Used in Moving 1
    public struct RotatingInformation
    {
        public Vector3 CenterOfRotation;
        public Vector3 RotatingDirection;
    }
    RotatingInformation RI;

    // Used in Moving 2
    private bool RightLeft;



    // Start is called before the first frame update
    void Start()
    {
        // 0 : Rotating, 1 : Left and Right, 2 : Rotating Around Player, 3: rotating circle
        MosquitoMovingChoice = Random.Range(0, 4);
        MqAudioSource = GetComponent<AudioSource>();

        // Change When Player Name Change!!!!!!!!!!!!!!!!!!!
        Player = GameObject.Find("Main Camera");

        OriPosition = transform.position;

        if (MosquitoMovingChoice == 0)
            RI = DecidingCenter();

        RightLeft = true;

        Debug.Log(MosquitoMovingChoice);
    }

    // Update is called once per frame
    void Update()
    {
        MosquitoMoving();
        if (MqAudioSource.isPlaying == false)
        {
            MqAudioSource.Play();
        }

        transform.LookAt(Player.transform.position);

    }

    public void MosquitoAttack()
    {
        /* Player HP_Down
         * Player.GetComponent<Script Name>().HP -= Damage;
         * GameObject.Find("GameManager").GetComponent<Script Name>().LifHUD();
         */

        //GameManager.instance.Player.HP--;

        /* Sound 
        * GetComponent<AudioSource>().Play(); - Choose Attack Sound Clip Seperately exist OR AudioSource Seperetely exist
        * AudioSource -> List<AudioSource> ** = GetComponents<AudioSource>() and check index
        * Clip -> this Script has sound clip and replace when attack occur
        */

        /* Attack
        * To Destroy Mosquito When Animator over
        * When attack occur Mosquito invincibility until attack animation over
        * Then Destroy Mosquito
        * I make this about Coroutine but this can be change about Animation State
        */
        StartCoroutine("Attack");

        /* Destroy
         * Destroy(this.transform.gameObject);
         * OR GameObject.Find("spawner").GetComponent<Script Name>().DestoryMosquito();
        */
    }

    IEnumerator Attack()
    {
        Animator animator = GetComponent<Animator>();
        int Attack = Animator.StringToHash("Attack");
        animator.SetBool(Attack, true);

        yield return new WaitForSeconds(0.4f);

        animator.SetBool(Attack, false);
    }

    public void MosquitoMoving()
    {
        if (MosquitoMovingChoice == 0)
        {
            transform.RotateAround(RI.CenterOfRotation, RI.RotatingDirection, Time.deltaTime * transform.localScale.magnitude * 200);
        }
        else if (MosquitoMovingChoice == 1)
        {
            if (RightLeft)
            {
                Debug.Log("right");
                transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
                if ((OriPosition - transform.position).magnitude > 0.3)
                    RightLeft = false;
            }
            else
            {
                Debug.Log("Left");
                transform.RotateAround(Player.transform.position, -Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
                if ((OriPosition - transform.position).magnitude < 0.01)
                    RightLeft = true;
            }
        }
        else if (MosquitoMovingChoice == 2)
        {
            transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
        }
        else if (MosquitoMovingChoice == 3)
        {
            float MagnitudeOfScale = transform.localScale.magnitude;
            transform.RotateAround(OriPosition - transform.localScale / 3, Vector3.back, Time.deltaTime * MagnitudeOfScale * 500);
        }
    }

    public RotatingInformation DecidingCenter()
    {
        float x, y, z;
        float OriPositionMag = OriPosition.magnitude;
        RotatingInformation TempRI = new RotatingInformation();

        x = Random.Range(-OriPosition.x, OriPosition.x);
        y = Random.Range(-OriPosition.y, OriPosition.y);
        z = Mathf.Sqrt(Mathf.Pow(OriPositionMag, 2) - OriPosition.x * x - OriPosition.y * y);
        Vector3 PPlane = new Vector3(x, y, z);
        Vector3 TempCOR = OriPosition - PPlane;
        Vector3 TempCOR2 = TempCOR / TempCOR.magnitude * (transform.localScale.magnitude);
        Vector3 CenterOfRotation = OriPosition + TempCOR2;
        Vector3 RotatingDirection = new Vector3(-TempCOR2.z, 1 / (-TempCOR2.y), TempCOR2.x);

        TempRI.CenterOfRotation = CenterOfRotation;
        TempRI.RotatingDirection = RotatingDirection;

        return TempRI;
    }

}
