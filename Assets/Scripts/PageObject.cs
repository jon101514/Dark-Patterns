/** Jonathan So, jds7523@rit.edu 
 * A PageObject is a highly-customizable element on our simulated webpage.
 * It can be interactible, visible, and flagged as a Dark Pattern.
 * 
 * PREREQUISITES: A Page singleton must be present in the scene.
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageObject : MonoBehaviour, IPointerClickHandler {

	public bool interactible; // Whether or not the user can click this PageObject.

	public int clickIndex; // The index sent to the Page upon click.

	// An array of Page indices where this object is visible.
	public int[] visibleRange;  // If the array size is zero, then this will be always visible.

	public bool isDark; // Whether or not this object can be red-flagged as a Dark Pattern.
	private bool hasBeenFlagged = false;

	private Image img; // The base image for this PageObject.
	private Text text; // The text object that could be a child of this PageObject. [text is optional]

	// Get the Image and Text components and make sure to disable the button if this isn't interactible.
	private void Awake() {
		img = GetComponent<Image>();
		text = transform.GetComponentInChildren<Text>();
		SetInteractible(interactible);
	}

	/** Setter for the interactible bool.
	 * param[newVal] - the new value we want to set it to.
	 */
	public void SetInteractible(bool newVal) {
		interactible = newVal;
		if (interactible) {
			GetComponent<Button>().enabled = true;
		} else {
			GetComponent<Button>().enabled = false;
		}
	}

	/** When clicked, update the Page by sending our clickIndex to it.
	 * Called by the attached Button script.
	 */
	public void Click() {
		Page.instance.UpdatePage(clickIndex);

	}

	/** Handles right-clicking for all "dark" objects.
	 * When you right click on a dark objet that hasn't been flagged yet, you will flag it red.
	 * Based on the following code: https://forum.unity.com/threads/can-the-ui-buttons-detect-a-right-mouse-click.279027/
	 */
	public void OnPointerClick(PointerEventData peData) {
		if (peData.button.Equals(PointerEventData.InputButton.Right) && isDark && !hasBeenFlagged) {
			hasBeenFlagged = true;
			if (img) {
				img.color = Color.red;
			}
		}
	}

	/** Called by the Page, this searches the visible range to see if this should be visible
	 * given the new page index. 
	 */
	public void UpdatePageObject() {
		ToggleVisibility(SearchVisibleRange());
	}

	/** Search to see if, given the page's new index, this should be visible.
	 *  return - a bool stating whether or not this PageObject should be visible.
	 */
	private bool SearchVisibleRange() {
		for (int i = 0; i < visibleRange.Length; i++) {
			if (visibleRange[i].Equals(Page.instance.index)) {
				return true;
			}
		}
		return false;
	}


	/** Enable or disable visibility of this object.
	 * If we're within visible range or we want this PageObject to always be visible, then enable the Image.
	 * Otherwise, we're out of visible range and disable the image.
	 * param[isVisible] - bool stating whether or not we want this to be visible.
	 */
	private void ToggleVisibility(bool isVisible) {
		if (isVisible || visibleRange.Length <= 0) {
			img.enabled = true;
			if (text) { text.enabled = true; }

		} else {
			img.enabled = false;
			if (text) { text.enabled = false; }
		}
	}
}
