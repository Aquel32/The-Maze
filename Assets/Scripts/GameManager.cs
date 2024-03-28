using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    private void Awake() { Instance = this; }

    public List<Player> playerList = new List<Player>();

    public GameObject playerPrefab;
    public Transform spawnsParent;

    public List<Item> items = new List<Item>();

    [SerializeField] private MapGenerator mapGenerator;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
            return;
        }

        photonView.RPC("CreateNewPlayerRPC", RpcTarget.AllBuffered);

        SpawnPlayer(Player.myPlayer);
    }

    [PunRPC]
    public void CreateNewPlayerRPC(PhotonMessageInfo photonMessageInfo)
    {
        Photon.Realtime.Player photonPlayer = photonMessageInfo.Sender;

        Player newPlayer = new Player();
        playerList.Add(newPlayer);

        newPlayer.photonPlayer = photonPlayer;

        if(PhotonNetwork.NickName == newPlayer.photonPlayer.NickName)
        {
            Player.myPlayer = newPlayer;
        }
    }

    public void SpawnPlayer(Player playerToSpawn)
    {
        GameObject newPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, GetRandomSpawn(), Quaternion.identity);

        //Initialize local scripts
        newPlayerObject.GetComponent<PlayerInitializer>().enabled = true;

        photonView.RPC("SpawnPlayerRPC", RpcTarget.AllBuffered, newPlayerObject.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void SpawnPlayerRPC(int objectViewID, PhotonMessageInfo pmi)
    {
        GameObject playerToSpawnObject = PhotonView.Find(objectViewID).gameObject;
        Player playerToSpawn = Player.FindPlayer(pmi.Sender);

        playerToSpawnObject.name = "Player " + playerToSpawn.photonPlayer.NickName;
        playerToSpawn.playerObject = playerToSpawnObject;

        playerToSpawnObject.GetComponent<PlayerReference>().referencedPlayer = playerToSpawn;

        if(PhotonNetwork.IsMasterClient && !mapGenerator.IsMapGenerated)
        {
            mapGenerator.GenerateMap();
        }
    }

    public void DespawnPlayer(Player playerToDespawn)
    {
        photonView.RPC("DespawnPlayerRPC", RpcTarget.AllBuffered, playerToDespawn.playerObject.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void DespawnPlayerRPC(int goViewId)
    {
        Destroy(PhotonView.Find(goViewId).gameObject);
    }

    public Vector3 GetRandomSpawn()
    {
        int generatedIndex = Random.Range(0, spawnsParent.childCount);
        return spawnsParent.GetChild(generatedIndex).position;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        DespawnPlayer(Player.FindPlayer(otherPlayer));
        playerList.Remove(Player.FindPlayer(otherPlayer));
    }

    public Item getItemFromName(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == name) return items[i];
        }

        return null;
    }    
}
