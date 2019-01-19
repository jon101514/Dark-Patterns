/** Jonathan So, jds7523@rit.edu 
 * Victory Fruit is created when the player successfully completes a mission; they have randomized sprites and move on the screen like confetti.
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryFruit : MonoBehaviour {

	public Sprite[] fruits; // The different fruit sprites this can choose from.

	private Rigidbody2D rb;
	private Image img;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		img = GetComponent<Image>();
		img.sprite = fruits[Random.Range(0, fruits.Length - 1)];
	}

	private void Start() {
		transform.position = new Vector2(Random.Range(-600f, 600f), Random.Range(400f, 600f));
		rb.velocity = (new Vector2(Random.Range(-1f, 1f) * 512f, Random.Range(-1f, 1f) * 512f));
	}
}
