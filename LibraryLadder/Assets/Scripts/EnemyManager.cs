using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public List<GameObject> targets = new List<GameObject>();
    public GameObject currentTarget;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        var enemiesarray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemiesarray)
        {
            targets.Add(e);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
