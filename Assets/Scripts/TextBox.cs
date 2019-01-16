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

    private bool fForward = false; //Is the fast forward button toggled or not
    private Button fForwardButton; //Reference to the fast forward button

	/** Get the nametag, textfield, fast forward button, and image display children, then set the name.	 
	 */
	private void Awake() {
		nametag = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		textfield = transform.GetChild(1).GetComponent<Text>();
		imgDisplay = transform.GetChild(2).GetComponent<Image>();
		nametag.text = name;

        fForwardButton = transform.GetChild(3).GetComponent<Button>();
	}

	// Upon startup, begin displaying text.
	private void Start() {
		currCoroutine = StartCoroutine(DisplayText(0));
        imgDisplay.sprite = images[0];
    }

	// Stop the current coroutine and then display the previous piece of dialogue.
	public void Back() {
		if (currIndex - 1 < 0) { // Bounds checking
			return;
		}

        ChangeIndex(currIndex - 1);
    }

	// Display the whole piece of dialogue.
	public void FastForward() {
        if(fForward == false)
        {
            fForward = true;
            StopCoroutine(currCoroutine);
            textfield.text = dialogue[currIndex];

            ColorBlock newColor = new ColorBlock();
            newColor= fForwardButton.colors;
            newColor.highlightedColor = new Color(.81f, .81f, .81f);
            newColor.normalColor = new Color(.85f, .85f, .85f);
            fForwardButton.colors = newColor;
        }
        else
        {
            fForward = false;

            ColorBlock newColor = new ColorBlock();
            newColor = fForwardButton.colors;
            newColor.highlightedColor = new Color(.96f, .96f, .96f);
            newColor.normalColor = new Color(1, 1, 1);
            fForwardButton.colors = newColor;
        }

	}

	// Stop the current coroutine and then display the next piece of dialogue.
	public void Next() {
		if (currIndex + 1 >= dialogue.Length) { // Bounds checking
			SceneManager.LoadScene("NaturalSupports");
			return;
		}

        ChangeIndex(currIndex + 1);
	}

    //Function to handle the beginning of the text display based on fast forward state
    private void ChangeIndex(int newIndex)
    {
        //stop the current coroutine and set the new index
        StopCoroutine(currCoroutine);
        currIndex = newIndex;

        // Display Image
        imgDisplay.sprite = images[currIndex];

        //Either start the coroutine to slowly display the text or show it all at once
        if (fForward)
        {
            textfield.text = dialogue[currIndex];
        }
        else
        {
            currCoroutine = StartCoroutine(DisplayText(currIndex));
        }

    }

	/** Clear the textfield and then gradually print the text of the current dialogue piece to the screen.
	 * param[index] - the index of the dialogue array we want to display.
	 */
	private IEnumerator DisplayText(int index) {
		textfield.text = "";
		string currString = dialogue[index];
		// Display text
		for (int i = 0; i < currString.Length; i++) {
			textfield.text += currString[i];
			yield return new WaitForSeconds(PRINT_TIME);
		}
	}


}
