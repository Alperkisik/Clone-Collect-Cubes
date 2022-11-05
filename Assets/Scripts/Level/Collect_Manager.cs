using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Manager : MonoBehaviour
{
    public enum CollectZones
    {
        Player_Collect_Zone, Opponent_Collect_Zone
    }

    [SerializeField] Material material;
    [SerializeField] Level_Manager levelManager;
    [SerializeField] CollectZones collectZoneType;
    [SerializeField] Collectable_Manager col_manager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collect Manager -> Cube triggered the area");

        if (other.gameObject.tag == "Cube") 
        {
            other.gameObject.transform.parent = this.transform;
            //Collected cube material is changed to yellow
            other.gameObject.GetComponent<MeshRenderer>().material = material;
            //Collected cube is pulled to middle
            PulltoMiddle();
            //to prevent collision between collected cubes and player
            other.gameObject.layer = 8;

            if (collectZoneType == CollectZones.Player_Collect_Zone) levelManager.IncreasePlayerScore(1);
            else levelManager.IncreaseOpponentScore(1);

            col_manager.CubeCollected();
        } 
    }

    void PulltoMiddle()
    {

    }
}
