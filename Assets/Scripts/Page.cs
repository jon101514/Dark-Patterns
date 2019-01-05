/** Jonathan So, jds7523@rit.edu 
 * The Page singleton manages all of the PageObjects, keeping track of their arrangement to make them all look
 * like they're different elements of a webpage.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {

	public static Page instance; // Singleton design pattern.

	public int index = 0; // The current index of the page. Indices tell us what "page" we're on.

	private List<PageObject> pageObjects; // List of all of the PageObjects that make up our simulated website.

	// Set up the Singleton object.
	private void Start() {
		if (instance == null) {
			instance = this;
		}
		pageObjects = new List<PageObject>(Object.FindObjectsOfType<PageObject>()); // Make a new list of all of the PageObjects.
		UpdatePage(0);
	}

	/** When a PageObject is clicked, it calls this UpdatePage function with
	 * the new index we go to. It then notifies all PageObjects of the change.
	 *  param[newIndex] - the new index of the page.
	 */
	public void UpdatePage(int newIndex) {
		index = newIndex;
		for (int i = 0; i < pageObjects.Count; i++) {
			pageObjects[i].UpdatePageObject();
		}
	}
}
