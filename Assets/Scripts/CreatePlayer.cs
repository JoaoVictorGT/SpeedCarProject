using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreatePlayer : MonoBehaviourPunCallbacks 
{

    //public RPGCamera Camera;
    //public CameraTracking Camera;

    public override void OnJoinedRoom()
    {
        CreatePlayerObject();
    }

    void CreatePlayerObject()
    {
        Vector3 position = new Vector3( 3f, -1.5f, 5f );

        GameObject newPlayerObject = PhotonNetwork.Instantiate( "CarCamaroAmarelo", position, Quaternion.identity, 0 );

    }
}
