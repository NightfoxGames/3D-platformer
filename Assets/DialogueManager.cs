using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
    public Image image;
    public GameObject _dialogue;

	public Animator animator;

	private Queue<string> sentences;
    private Queue<Sprite> images;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
        images = new Queue<Sprite>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
        _dialogue.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
         Time.timeScale = .5f;
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();
        images.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
            
		}
        foreach (Sprite item in dialogue.images)
        {
            images.Enqueue(item);
        }
		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
        Sprite _image = images.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, _image));
	}

	IEnumerator TypeSentence (string sentence, Sprite __image)
	{
		dialogueText.text = "";
        if(__image != null)
        image.sprite = __image;
        else{
            image.gameObject.SetActive(false);
        }
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
        FindObjectOfType<PlayerController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
         Time.timeScale = 1f;
	}

}