using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;

    GameObject healthBarUI;
    public Slider slider;
    public GameObject targetingImage;
    public GameObject canvas;

    void Start()
    {
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyManager.instance.currentTarget == this.gameObject)
        {
            targetingImage.SetActive(true);
        }
        else
        {
            targetingImage.SetActive(false);

        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        transform.LookAt(Camera.main.transform);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Book")
        {
            health -= 10;
            slider.value = health;
            EnemyManager.instance.targets.Remove(this.gameObject);
        }
    }
}
