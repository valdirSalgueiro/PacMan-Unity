using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPill : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
