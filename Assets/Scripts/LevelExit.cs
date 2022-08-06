using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public GameObject fakeObject;
    public Animator anim;
    public int index = 0;
    bool isClosed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          if(PlayerPrefs.GetInt(index+"_CLOSED") == 1)
            isClosed = false;
            else
            isClosed = true;
            gameObject.SetActive(isClosed);
            fakeObject.SetActive(!isClosed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            anim.SetTrigger("Hit");

            StartCoroutine(GameManager.instance.LevelEndCo(index));
            
            
          
            
        }
    }
}
