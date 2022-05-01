using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    public GameObject Player;

    // About Mosquito
    protected int MosquitoMovingChoice;
    public AudioSource MqAudioSource;
    public float OriginalMosquitoHP = 1;
    public float MosquitoHP = 1;
    public int MosquitoDamage = 1;

    // Used in Heated
    public bool HeatedAnimationStart; //= true; : OnEnable -> True, Because if Mosquito Object OnEnable, reset
    public AudioClip Temp;

    // Used in Moving
    protected Vector3 OriPosition;
    // To Change Mosquito's Center Of Rotation, Get ArCamera Position #Update3
    protected Transform ArCameraTransform;
    protected Vector3 ArCameraOriginPosition;

    // Used in Moving 1
    public struct RotatingInformation
    {
        public Vector3 CenterOfRotation;
        public Vector3 RotatingDirection;
    }
    protected RotatingInformation RI;

    // Used in Moving 2
    protected bool RightLeft;



    // Start is called before the first frame update
    void OnEnable()
    {
        // 0 : Rotating, 1 : Left and Right, 2 : Rotating Around Player, 3: rotating circle
        MosquitoMovingChoice = Random.Range(0, 4);
        MqAudioSource = GetComponent<AudioSource>();

        // Change When Player Name Change!
        Player = GameObject.Find("Player");

        OriPosition = transform.position;

        if (MosquitoMovingChoice == 0)
            RI = DecidingCenter();

        RightLeft = true;

        Debug.Log(MosquitoMovingChoice);


        // #Update3
        ArCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        ArCameraOriginPosition = ArCameraTransform.position;

        // #Object pooling version
        MosquitoHP = OriginalMosquitoHP;
        if (Temp != null)
        {
            MqAudioSource.clip = Temp;
            MqAudioSource.Play();
        }
        HeatedAnimationStart = true;
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


        // #Update3
        Vector3 CameraMoveVector = ArCameraTransform.position - ArCameraOriginPosition;
        OriPosition += CameraMoveVector;
        RI.CenterOfRotation += CameraMoveVector;
        transform.position += CameraMoveVector;
        ArCameraOriginPosition = ArCameraTransform.position;

    }


    // Attack
    public void ReadyToAttack()
    {
        /* In GameManager Add
         // When, must be in field!
         public bool 1Second = true; 
        
         // When, TimeOperation ()
         if (TimeLimit <= 0.7f && 1Second == true ) {
            for(int i = spawner.Mosquitos.Count - 1; i >= 0; i--)
            {
                spawner.Mosquitos[i].GetComponent<MosquitoController>().ReadyToAttack();
             }
            1Second = false;
         }

         // When, TimeOver ()
         1Second = true;
         */

        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Animator animator = GetComponent<Animator>();
        int Attack = Animator.StringToHash("Attack");
        animator.SetBool(Attack, true);

        yield return new WaitForSeconds(0.3f);

        /* If it have Sound 
         * GetComponent<AudioSource>().Play(); - Choose Attack Sound Clip Seperately exist OR AudioSource Seperetely exist
         * AudioSource -> List<AudioSource> ** = GetComponents<AudioSource>() and check index
         * Clip -> this Script has sound clip and replace when attack occur
         */
    }

    public void MosquitoAttack()
    {
        /* Player HP_Down
         * Player.GetComponent<Script Name>().HP -= Damage;
         * GameObject.Find("GameManager").GetComponent<Script Name>().LifHUD();
         */

        Player.GetComponent<Player>().HP -= MosquitoDamage;

        // Attacked Sound Start
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().AttackedStart();

        if (GameManager.instance.vibrateSwitch == true)
            Handheld.Vibrate();

        /* Attack
        * To Destroy Mosquito When Animator over
        * When attack occur Mosquito invincibility until attack animation over
        * Then Destroy Mosquito
        * I make this about Coroutine but this can be change about Animation State
        */
    }


    // Heated - Player 
    public void MosquitoHeated()
    {
        // In 'Player' Script, After the Debug.Log("Heating") - MosquitoController create,
        // raycastedMosquito.MosquitoHeated()
        // But, If Mosquito is lot, all animation change?? (Guess)
        // Change Other Sound Clip And Play & ReChange Original Sound ??
        if (HeatedAnimationStart)
        {
            StartCoroutine("Heated");
            HeatedAnimationStart = false; // Q. When Other Animation occur, Animation transitions have delay -> About duration. Solved! 
        }
    }

    IEnumerator Heated()
    {
        // Q. Heated Mosquito's HP down Intergrated??
        Animator animator = GetComponent<Animator>();
        int Damaged = Animator.StringToHash("Damaged");
        animator.SetBool(Damaged, true);

        Temp = MqAudioSource.clip;
        MqAudioSource.clip = GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().MosquitoHeated;
        MqAudioSource.Play();

        yield return new WaitForSeconds(0.2f);

        MqAudioSource.clip = Temp;
        MqAudioSource.Play();

        animator.SetBool(Damaged, false);
        HeatedAnimationStart = true;
    }

    // Moving
    public virtual void MosquitoMoving()
    {
        if (MosquitoMovingChoice == 0) // Moving 0
        {
            transform.RotateAround(RI.CenterOfRotation, RI.RotatingDirection, Time.deltaTime * transform.localScale.magnitude * 200);
        }
        else if (MosquitoMovingChoice == 1) // Moving 1
        {
            if (RightLeft)
            {
                //Debug.Log("right");
                transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
                if ((OriPosition - transform.position).magnitude > 0.3)
                    RightLeft = false;
            }
            else
            {
                //Debug.Log("Left");
                transform.RotateAround(Player.transform.position, -Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
                if ((OriPosition - transform.position).magnitude < 0.01)
                    RightLeft = true;
            }
        }
        else if (MosquitoMovingChoice == 2) // Moving 2
        {
            transform.RotateAround(Player.transform.position, Vector3.up, Time.deltaTime * transform.localScale.magnitude * 30);
        }
        else if (MosquitoMovingChoice == 3) // Moving 3
        {
            float MagnitudeOfScale = transform.localScale.magnitude;
            transform.RotateAround(OriPosition - transform.localScale / 3, Vector3.back, Time.deltaTime * MagnitudeOfScale * 500);
        }
    }


    // Used in Moving 0
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
