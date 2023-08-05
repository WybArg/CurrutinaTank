using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float timeLifeMissile;
    public float timeSlowMotion;

    public delegate void DelTank();
    public static DelTank OnEventTank;

    private void Start()
    {
        StartCoroutine(SlowMotion());
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 position = Vector3.forward;
        transform.Translate(position * Time.timeScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        StartCoroutine(DestroyerMissile(timeLifeMissile));
    }

    IEnumerator DestroyerMissile(float timeLifeMissile)
    {
        yield return new WaitForSeconds(timeLifeMissile);
        OnEventTank?.Invoke();
        Destroy(this.gameObject);
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.02f;
        yield return new WaitForSeconds(timeSlowMotion / 100);
        Time.timeScale = 1f;
    }


}
