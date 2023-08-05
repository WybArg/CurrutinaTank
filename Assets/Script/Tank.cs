using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{

    public Transform target;
    private float angle;
    public float timeInterval;
    public float minDistance;
    [Space]
    private bool moveInDirectionTarget = false;
    public float speed;
    private bool firstIntent = true;
    public Animator myAni;
    public string clipTankName;
    private bool secondEnter = false;
    private float time;
    private bool continueTime = true;
    private bool delayTime = false;
    [Space]
    public GameObject missile;
    public Transform positionMissile;
    [Space]
    public Text textTime;

    private void Start()
    {
        Missile.OnEventTank += ClipTankHandler;
    }


    void Update()
    {
        if (target != null && firstIntent) StartCoroutine(TargetDirection(timeInterval));

        Move();


        if (continueTime)
        {
            if (delayTime)
            {
                time += Time.deltaTime/timeInterval;
            }
            else
            {
                time += Time.deltaTime;
            }
            
            textTime.text = FormatTime(time);
        }
      

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (secondEnter)
            {
                moveInDirectionTarget = true;
            }
            else
            {
                StopAllCoroutines();
                moveInDirectionTarget = false;
            }
            secondEnter = !secondEnter;
            continueTime = !continueTime;
        }
    }


    IEnumerator TargetDirection(float timeInterval)
    {
        firstIntent = false;

        Vector3 direction = target.position - transform.position;

        angle = Vector3.SignedAngle(direction, transform.forward, Vector3.down);

        transform.Rotate(0, angle, 0);

        yield return new WaitForSeconds(timeInterval);

        moveInDirectionTarget = true;
    }

    public void Move()
    {
        if (!moveInDirectionTarget) return;
        if (target == null) return;

        if (Vector3.Distance(target.position, this.transform.position) > minDistance)
        {
            Vector3 position = Vector3.forward;
            transform.Translate(position * speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(CreateMissile());
            moveInDirectionTarget = false;
        }
    }



    IEnumerator CreateMissile()
    {
        yield return new WaitForSeconds(timeInterval);
        Instantiate(missile, positionMissile.position, positionMissile.rotation);
        StartCoroutine(SlowMotionTime(timeInterval));
    }


    public void ClipTankHandler()
    {
        StartCoroutine(DestroyerTank(timeInterval));
    }


    IEnumerator DestroyerTank(float timeInterval)
    {
        myAni.SetTrigger(clipTankName);
      
       
        yield return new WaitForSeconds(timeInterval);
        Destroy(this.gameObject);
    }


    public string FormatTime(float time)
    {
        int minute = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliSeconds = (int)(time * 1000f) % 1000;

        return string.Format("{0:00}:{1:00}:{2:000}", minute, seconds, milliSeconds);
    }

    IEnumerator SlowMotionTime(float timeInterval)
    {
        delayTime = true;
        yield return new WaitForSeconds(timeInterval);
        delayTime = false;
    }
}
