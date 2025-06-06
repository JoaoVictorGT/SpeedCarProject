using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnector : MonoBehaviourPunCallbacks // Importante herdar de MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Conectando ao Photon...");
        PhotonNetwork.ConnectUsingSettings(); // Conecta usando as configurações do PhotonServerSettings
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao Master Server do Photon!");
        // Agora você pode, por exemplo, entrar em um Lobby ou criar/entrar em uma sala.
        // PhotonNetwork.JoinLobby(); // Opcional: Entrar em um lobby para ver salas disponíveis
        // PhotonNetwork.JoinOrCreateRoom("MinhaSala", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default); // Exemplo de como criar/entrar em uma sala
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Desconectado do Photon: {0}", cause);
    }

    // Opcional: Callback para quando se junta a uma sala
    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala: " + PhotonNetwork.CurrentRoom.Name);
        // Lógica para quando o jogador entra na sala (ex: instanciar o personagem do jogador)
    }

    // Opcional: Callback para quando falha ao entrar/criar uma sala
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Falha ao entrar na sala: {0} - {1}", returnCode, message);
    }
}