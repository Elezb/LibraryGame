using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBooksFromShelves : MonoBehaviour
{
    GameObject[] enemies;
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    [SerializeField] GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        AddVisibleEnemiesToTargetList();
        SwitchToNextTarget();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Book")
        {
            if (currentTarget != null)
            {
                // should be a determined path to target, not physics
                Vector3 direction = currentTarget.transform.position - collider.transform.position;
                collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * 80f + Vector3.up * 70f);
            }
            else
            {
                Vector3 direction = this.transform.position - collider.transform.position;
                collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * 80f + Vector3.up * 70f);
            }
        }
    }

    void AddVisibleEnemiesToTargetList()
    {
        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen && !targets.Contains(enemy))
            {
                targets.Add(enemy);
            }
            else if (!onScreen && targets.Contains(enemy))
            {
                targets.Remove(enemy);
            }
        }

    }

    void SwitchToNextTarget()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (currentTarget != null && targets.Count > 1)
            {
                int indexCurrent = targets.IndexOf(currentTarget);
                int indexNext = targets.Count > indexCurrent + 1 ? indexCurrent + 1 : 0;
                currentTarget = targets[indexNext];
            }
            else
            {
                currentTarget = targets[0] != null ? targets[0] : null;
            }
        }

        if (currentTarget == null && targets.Count >= 1)
        {
            currentTarget = targets[0];
        }
    }
}
