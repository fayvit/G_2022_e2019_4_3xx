using UnityEngine;
using System.Collections;

public class EscalaParaSabre : MonoBehaviour {

	[SerializeField] private float vel=10;
	[SerializeField] private Vector3 scaleFull = new Vector3(1, 1, 1);
	[SerializeField] private Vector3 scaleTiny = new Vector3(.1f, .1f, .1f);

	// Update is called once per frame
	void Update () {
		if (transform.localScale.sqrMagnitude < .97f*scaleFull.sqrMagnitude)
			transform.localScale = Vector3.Lerp(
				transform.localScale,
				scaleFull,
				Time.deltaTime * vel
				);
		else
			transform.localScale = scaleTiny;
	}
}
