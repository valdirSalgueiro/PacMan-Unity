using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPill : MonoBehaviour
{
    public AudioClip eatClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddScore(50);
            SoundManager.instance.PlaySingle(eatClip, 1);
            Destroy(gameObject);
            var ghostsGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject ghostsGameObject in ghostsGameObjects)
            {
                var ghost = ghostsGameObject.GetComponent<Ghost>();
                ghost.SetFrightened();
            }
        }
    }
}
