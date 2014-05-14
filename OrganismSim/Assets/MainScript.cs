using UnityEngine;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {
	public GameObject organsim;
	public GameObject food;
	
	private GameObject[] organisms = new GameObject[16];
	private GameObject[] foods = new GameObject[16];
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < organisms.Length; i++) {
			organisms[i] = Instantiate(organsim, new Vector3(Random.Range(-50,50), Random.Range (-50, 50), 0), Quaternion.identity) as GameObject;

		}
		for (int i = 0; i < foods.Length; i++) {
			foods[i] = Instantiate(food, new Vector3(Random.Range(-50,50), Random.Range (-50, 50), 0), Quaternion.identity) as GameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < organisms.Length; i++) {
			OrganismDriver driver = organisms[i].GetComponent<OrganismDriver>();
			float minDistance = Vector3.Distance(organisms[i].transform.position, foods[0].transform.position);
			int closest = 0;

			for (int j = 1; j < foods.Length; j++) {
				float currDistance = Vector3.Distance(organisms[i].transform.position, foods[0].transform.position);
				if (currDistance < minDistance) {
					minDistance = currDistance;
					closest = j;
				}
			}

			driver.ComputeNetwork(foods[closest].transform.position);
			driver.UpdateLocation();



		}
	}
}
