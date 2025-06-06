using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class NetWorkController : MonoBehaviourPunCallbacks
{
    [Header("GO")]
    public GameObject loginGO;       
    public GameObject partidasGO;    
    public GameObject informactionGO; 

    [Header("Player")]
    public InputField playerNameInput; 
    string playerNameTemp;
    public GameObject myPlayerPrefab;

    [Header("Room")]
    public InputField roomNameInput;

    [Header("InforMaction")]
    public Text infoText;
    public Text playerListText;

    void Start()
    {
        // Lógica para a cena MainMenu (nickname)
        if (SceneManager.GetActiveScene().name == "MainMenu") 
        {
            if (playerNameInput != null)
            {
                playerNameTemp = "Player" + Random.Range(1000, 10000);
                playerNameInput.text = playerNameTemp;
            }
            if (loginGO != null) loginGO.SetActive(true);
            if (partidasGO != null) partidasGO.SetActive(false);
            if (informactionGO != null) informactionGO.SetActive(false);
        }
        // Lógica para a cena LoginScene (criar/entrar sala)
        else if (SceneManager.GetActiveScene().name == "LoginScene") 
        {
            if (roomNameInput != null)
            {
                roomNameInput.text = "Room" + Random.Range(1000, 10000);
            }
            if (loginGO != null) loginGO.SetActive(false);
            
            if (partidasGO != null && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.Server == ServerConnection.MasterServer)
            {
                partidasGO.SetActive(true);
            } else if (partidasGO != null) {
                partidasGO.SetActive(false); 
            }
            if (informactionGO != null) informactionGO.SetActive(false);
        }
        // Lógica para a cena GameScene
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            
            if (loginGO != null) loginGO.SetActive(false);
            if (partidasGO != null) partidasGO.SetActive(false);
        }
    }

    public void BtLogin()
    {
        if (playerNameInput != null && playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = playerNameTemp;
        }
        Debug.Log("Usuario definido como: " + PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
        if (loginGO != null) loginGO.SetActive(false);
    }

    public void BtCriarSala()
    {
        Debug.Log("BtCriarSala FOI CLICADO! Tentando criar/entrar na sala com nome: " + (roomNameInput != null ? roomNameInput.text : "INPUT NULO"));
        if (roomNameInput != null && roomNameInput.text != "")
        {
            string roomNameTemp = roomNameInput.text;
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 20 };
            PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
        }
        else
        {
            Debug.LogError("O nome da sala não pode ser vazio!");
        }
    }
    
    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("OnConnected - Conectado ao servidor (nível baixo)");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster - Conectado ao Master Server");
        Debug.Log("Servidor: " + PhotonNetwork.CloudRegion + " / Ping: " + PhotonNetwork.GetPing());

        // Se estamos na cena MainMenu (nickname) e conectamos ao Master, carregamos a LoginScene (criar sala)
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("Carregando LoginScene (tela de criar sala)...");
            SceneManager.LoadScene("LoginScene"); 
        }
        // Se já estamos na LoginScene (criar sala) e reconectamos/conectamos
        else if (SceneManager.GetActiveScene().name == "LoginScene")
        {
            if (partidasGO != null) partidasGO.SetActive(true); // Ativa a UI de criar sala
            if (loginGO != null) loginGO.SetActive(false);
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("OnJoinedLobby - Entrou no Lobby");
        // Para o botão "Sala Aleatória", tentamos entrar numa sala aleatória
        if (isTryingToJoinRandomRoom) { 
            PhotonNetwork.JoinRandomRoom();
            isTryingToJoinRandomRoom = false; 
        }
    }
    
    private bool isTryingToJoinRandomRoom = false;

    public void BtBuscarPartidaRapida() 
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            isTryingToJoinRandomRoom = true;
            PhotonNetwork.JoinLobby(); 
        } else {
            Debug.LogWarning("Não conectado ao Master Server para buscar partida rápida.");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.LogWarning($"OnJoinRandomFailed: Falhou em entrar em sala aleatória. Código: {returnCode} Mensagem: {message}. Criando nova sala...");
        string roomTemp = "SalaAleatoria" + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomTemp, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("CALLBACK OnJoinedRoom INICIADO. Cena atual: " + SceneManager.GetActiveScene().name); // LOG ATUALIZADO
        base.OnJoinedRoom();
        Debug.Log("OnJoinedRoom - Entrou na Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Jogadores na Sala: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (SceneManager.GetActiveScene().name == "LoginScene") 
        {
            Debug.Log("Condição para carregar GameScene ATINGIDA. Carregando cena do jogo: GameScene...");
            SceneManager.LoadScene("GameScene"); 
        }
        
        else if (SceneManager.GetActiveScene().name == "GameScene") 
        {
            Debug.Log("Já estamos na GameScene. Lógica de OnJoinedRoom para GameScene executada.");
            
            bool localPlayerAlreadyInstantiated = false; 
            
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject p in players) {
                PhotonView pv = p.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine) {
                    localPlayerAlreadyInstantiated = true;
                    break;
                }
            }

            if (!localPlayerAlreadyInstantiated) {
                if (myPlayerPrefab != null)
                {
                    Debug.Log("Instanciando jogador na GameScene.");
                    PhotonNetwork.Instantiate(myPlayerPrefab.name, Vector3.zero, Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogError("myPlayerPrefab não está atribuído no NetWorkController!");
                }
            }

            if (informactionGO != null) informactionGO.SetActive(true);
            if (partidasGO != null) partidasGO.SetActive(false);
            if (loginGO != null) loginGO.SetActive(false);
            if (infoText != null) infoText.text = ("Jogador: " + PhotonNetwork.NickName + " Sala: " + PhotonNetwork.CurrentRoom.Name);
            // UpdatePlayerListUI(); 
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogWarning($"OnDisconnected: Desconectado. Causa: {cause}");
        SceneManager.LoadScene("MainMenu");
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.NickName} entrou na sala.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"{otherPlayer.NickName} saiu da sala.");
    }
}