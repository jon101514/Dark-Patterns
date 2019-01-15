/** Jonathan So, jds7523@rit.edu 
 * The TextBox displays text that the host will speak. Players interact with this via buttons.
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextBox : MonoBehaviour {

	public string name; // Name of the host.
	[TextArea(2, 4)]
	public string[] dialogue; // Pieces of text; the "dialogue" the host speaks.
	public Sprite[] images; // The images to display alongside each piece of dialogue.

	private Text nametag; // Displays the Name of the host.
	private Text textfield; // Displays text to post.
	private Image imgDisplay; // Displays the current image.
	private int currIndex = 0; // Index of the dialogue array we're on.
	private Coroutine currCoroutine; // The current coroutine we're running.

	private const float PRINT_TIME = 1/45f; // Time it takes in-between printing characters of text to the textbox.

	/** Get the nametag, textfield, and image display children, then set the name.	 
	 */
	private void Awake() {
		nametag = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		textfield = transform.GetChild(1).GetComponent<Text>();
		imgDisplay = transform.GetChild(2).GetComponent<Image>();
		nametag.text = name;
	}

	// Upon startup, begin displaying text.
	private void Start() {
		currCoroutine = StartCoroutine(DisplayText(0));
	}

	// Stop the current coroutine and then display the previous piece of dialogue.
	public void Back() {
		if (currIndex - 1 < 0) { // Bounds checking
			return;
		}
		StopCoroutine(currCoroutine);
		currIndex--;
		currCoroutine = StartCoroutine(DisplayText(currIndex));
	}

	// Display the whole piece of dialogue.
	public void FastForward() {
		StopCoroutine(currCoroutine);
		textfield.text = dialogue[currIndex];
	}

	// Stop the current coroutine and then display the next piece of dialogue.
	public void Next() {
		if (currIndex + 1 >= dialogue.Length) { // Bounds checking
			SceneManager.LoadScene("Page Demo");
			return;
		}
		StopCoroutine(currCoroutine);
		currIndex++;
		currCoroutine = StartCoroutine(DisplayText(currIndex));
	}

	/** Clear the textfield and then gradually print the text of the current dialogue piece to the screen.
	 * param[index] - the index of the dialogue array we want to display.
	 */
	private IEnumerator DisplayText(int index) {
		textfield.text = "";
		string currString = dialogue[index];
		// Display Image
		imgDisplay.sprite = images[index];
		// Display text
		for (int i = 0; i < currString.Length; i++) {
			textfield.text += currString[i];
			yield return new WaitForSeconds(PRINT_TIME);
		}
	}


}
