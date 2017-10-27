using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {
	public GameObject[] availableRooms;
	public List<GameObject> currentRooms;
	private float screenWidthInPoints;

	void Start() {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
	}

	void AddRoom(float farhtestRoomEndX)
	{
    // Pick a random index of the room to generate
    int randomRoomIndex = Random.Range(0, availableRooms.Length);

    //Creates a room object form the array of available rooms under the random index aboves
    GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

    //3
    float roomWidth = room.transform.Find("Day").localScale.x;

    // //4
    // float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;
		//
    // //5
    // room.transform.position = new Vector3(roomCenter, 0, 0);
		//
    // //6
    // currentRooms.Add(room);
	}

}
