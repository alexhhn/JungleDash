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

	void FixedUpdate() {
		GenerateRoomIfRequired();
	}

	void AddRoom(float farhtestRoomEndX) {
    // Pick a random index of the room to generate
    int randomRoomIndex = Random.Range(0, availableRooms.Length);

    //Creates a room object form the array of available rooms under the random index aboves
    GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

    float roomWidth = room.transform.Find("Ground").localScale.x;
		
    float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

    room.transform.position = new Vector3(roomCenter, 0, 0);

		currentRooms.Add(room);
	}


	void GenerateRoomIfRequired(){

	List<GameObject> roomsToRemove = new List<GameObject>();

    bool addRooms = true;

   
    float playerX = transform.position.x;

    
    float removeRoomX = playerX - screenWidthInPoints;

    
    float addRoomX = playerX + screenWidthInPoints;

    
    float farthestRoomEndX = 0;

    foreach(var room in currentRooms)
    {
        
				float roomWidth = room.transform.Find("Ground").localScale.x;
        float roomStartX = room.transform.position.x - (roomWidth * 0.5f) -1;
        float roomEndX = roomStartX + roomWidth;

        
        if (roomStartX > addRoomX)
            addRooms = false;

        
        if (roomEndX < removeRoomX)
            roomsToRemove.Add(room);

        
        farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
    }

    //11
    foreach(var room in roomsToRemove)
    {
        currentRooms.Remove(room);
        Destroy(room);
    }

    //12
    if (addRooms)
        AddRoom(farthestRoomEndX);
		}
}
