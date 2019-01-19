/** Jonathan So, jds7523@rit.edu 
 * The Page singleton manages all of the PageObjects, keeping track of their arrangement to make them all look
 * like they're different elements of a webpage.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Page : MonoBehaviour {

	public static Page instance; // Singleton design pattern.

	public int index = 0; // The current index of the page. Indices tell us what "page" we're on.
	public int winIndex; // The index that the player must reach in order to win. When we eventually have more complex win conditions, we need to tweak this.

	public VictoryFruit fruitPrefab; // Victory fruit prefab we make upon the player's win.
	public GameObject successText; // Text that says "Success".
	public Image blackScreen; // The black panel that helps us fade the scene out.

	private List<PageObject> pageObjects; // List of all of the PageObjects that make up our simulated website.

	// Set up the Singleton object.
	private void Start() {
		if (instance == null) {
			instance = this;
		}
		pageObjects = new List<PageObject>(Object.FindObjectsOfType<PageObject>()); // Make a new list of all of the PageObjects.
		UpdatePage(0);
		blackScreen.enabled = false;
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
		// The player has won, so communicate that to the player.
		if (index == winIndex) {
			StartCoroutine(PlayerWin());
		}
	}

	/** Called when the player wins, this plays a victory animation and then loads the next scene in build order.
	 * First, it spawns multiple fruit objects. Then, a "Success" piece of text shows. Finally, the scene fades out and we 
	 * go to the next scene.
	 */
	private IEnumerator PlayerWin() {
		blackScreen.enabled = true; // Enable this as soon as the player wins to prevent further interactions -- it blocks any further clicks.
		// Phase 1: Fruits
		int fruits = Random.Range(16, 32);
		for (int i = 0 ; i < fruits; i++) {
			GameObject fruit = Instantiate(fruitPrefab).gameObject;
			fruit.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
		}
		yield return new WaitForSeconds(1.0f);
		// Phase 2: Success Text
		successText.SetActive(true);
		while (successText.transform.position.y > 300f) { // Move it downwards
			successText.transform.position = new Vector2(successText.transform.position.x, successText.transform.position.y - 4.0f);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		// Phase 3: Fade to Next Scene
		float timer = 0;
		while (blackScreen.color.a < 1) { // Move it downwards
			blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer));
			timer += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(1.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene.
	}
}
