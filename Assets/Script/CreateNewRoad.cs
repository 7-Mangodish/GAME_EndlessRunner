using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewRoad : MonoBehaviour
{
    [SerializeField] private GameObject roadPref; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(roadPref, new Vector3(0, 0, 129), Quaternion.identity);
        }
    }
}
