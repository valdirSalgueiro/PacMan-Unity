using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    public AudioClip eatClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddScore(10);
            SoundManager.instance.PlaySingle(eatClip, 1);
            Destroy(gameObject);
            // incrementa score
        }
    }
}
