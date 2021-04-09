using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBooksFromShelves : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AddVisibleEnemiesToTargetList();
        SwitchToNextTarget();
    }

    void OnTriggerEnter(Collider collider)
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        if (collider.tag == "Book")
        {
            if (EnemyManager.instance.currentTarget != null)
            {
                // should be a determined path to target, not physics
                Vector3 direction = EnemyManager.instance.currentTarget.transform.position - collider.transform.position;
                collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * 150f + Vector3.up * 150f);
            }
            else
            {
                Vector3 direction = this.transform.position - collider.transform.position;
                collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * 150f + Vector3.up * 150f);
            }
        }
        //  }
    }

    void AddVisibleEnemiesToTargetList()
    {
        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen && !EnemyManager.instance.targets.Contains(enemy))
            {
                EnemyManager.instance.targets.Add(enemy);
            }
            else if (!onScreen && EnemyManager.instance.targets.Contains(enemy))
            {
                EnemyManager.instance.targets.Remove(enemy);
            }
        }

    }

    void SwitchToNextTarget()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (EnemyManager.instance.currentTarget != null && EnemyManager.instance.targets.Count > 1)
            {
                int indexCurrent = EnemyManager.instance.targets.IndexOf(EnemyManager.instance.currentTarget);
                int indexNext = EnemyManager.instance.targets.Count > indexCurrent + 1 ? indexCurrent + 1 : 0;
                EnemyManager.instance.currentTarget = EnemyManager.instance.targets[indexNext];
            }
            else
            {
                EnemyManager.instance.currentTarget = EnemyManager.instance.targets[0] != null ? EnemyManager.instance.targets[0] : null;
            }
        }

        if (EnemyManager.instance.currentTarget == null && EnemyManager.instance.targets.Count >= 1)
        {
            EnemyManager.instance.currentTarget = EnemyManager.instance.targets[0];
        }
    }
}
