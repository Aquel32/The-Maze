using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    private void Awake() { Instance = this; }

    public List<Player> playerList = new List<Player>();

    public Transform spawnsParent;

    [SerializeField] private GameObject playerScriptsHandler;

    public List<Item> itemList = new List<Item>();
    public List<Recipe> recipeList = new List<Recipe>();


    /*public float time;
    public int dayLenght;
    [SerializeField] private Transform sun;
    private float sunRotationMultiplier;
    private float timer;

    public float timeSpeed;

    [SerializeField] private Animator doorAnimator;*/


    void Start()
    {
        //sunRotationMultiplier = 1 / (dayLenght / 360f);
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
            return;
        }

        photonView.RPC("CreateNewPlayerRPC", RpcTarget.AllBuffered);

        SpawnPlayer(Player.myPlayer);
    }

    private void Update()
    {
        /*if (!PhotonNetwork.IsConnected) return;
        if (!PhotonNetwork.IsMasterClient) return;

        timer += Time.deltaTime * timeSpeed;
        time = (int)(timer);

        if (time > dayLenght)
        {
            timer = 0;
            time = 0;
        }

        if (time == 440) doorAnimator.SetBool("IsOpen", true);
        if (time == 0) doorAnimator.SetBool("IsOpen", false);

        sun.Rotate(Time.deltaTime * sunRotationMultiplier * timeSpeed, 0,0);*/
    }

    [PunRPC]
    public void CreateNewPlayerRPC(PhotonMessageInfo photonMessageInfo)
    {
        Photon.Realtime.Player photonPlayer = photonMessageInfo.Sender;

        Player newPlayer = new Player();
        playerList.Add(newPlayer);

        newPlayer.photonPlayer = photonPlayer;
        newPlayer.isAdmin = PhotonNetwork.IsMasterClient;

        if (PhotonNetwork.NickName == newPlayer.photonPlayer.NickName)
        {
            Player.myPlayer = newPlayer;
        }
    }

    public void SpawnPlayer(Player playerToSpawn)
    {
        GameObject newPlayerObject = PhotonNetwork.Instantiate("Player", GetRandomSpawn(), Quaternion.identity);

        playerScriptsHandler.SetActive(true);
        newPlayerObject.GetComponent<PlayerInitializer>().enabled = true;
        UiManager.Instance.ChangeCurrentPanel(Panels.None);

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
    }

    public void DespawnPlayer(Player playerToDespawn)
    {
        playerScriptsHandler.SetActive(false);
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
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == name) return itemList[i];
        }

        return null;
    }    
}
