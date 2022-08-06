using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject space;
	public void OnTrigger ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        FindObjectOfType<PlayerController>().enabled = false;
	}

    
    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player"){
            space.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space)){
                OnTrigger();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            space.SetActive(false);
            
        }
    }

}
