using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Console : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform responsesParent;
    public GameObject console;
    [SerializeField] private TextMeshProUGUI responsePrefab;

    bool state;

    private void Start()
    {
        state = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        if(!Player.myPlayer.isAdmin) return;

        if (Input.GetKeyDown(KeyCode.BackQuote)) 
        {
            ChangeState(!state);
        }
    }

    public void ChangeState(bool newState)
    {
        state = newState;
        Player.myPlayer.playerObject.GetComponent<UiManager>().ChangeCurrentPanel(Panels.Console);
    }

    public void SendButton()
    {
        if (inputField.text == string.Empty)
        {
            SendResponse("Empty command");
            return;
        }

        string command = inputField.text;
        inputField.text = string.Empty;

        string[] args = command.Split(' ');

        if (args[0] == "help")
        {
            SendResponse("Command List:");
            SendResponse("give [player_id] [item_id]");
            SendResponse("exp [player_id] [new_value]");
            SendResponse("setadmin [player_id] [true/false]");
            SendResponse("heal [player_id]");
            SendResponse("playerlist");
            SendResponse("itemlist");
            SendResponse("recipelist");
            SendResponse("unlockall");
            SendResponse("unlock [player_id] [recipe_id]");
        }
        else if(args[0] == "give")
        {
            if(args.Length <= 2)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId) && int.TryParse(args[2], out int itemId))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count && itemId >= 0 && itemId < GameManager.Instance.itemList.Count)
                {
                    photonView.RPC("GiveItemRPC", RpcTarget.AllBuffered, playerId, itemId);
                    SendResponse("Added " + GameManager.Instance.itemList[itemId].name + " for " + GameManager.Instance.playerList[playerId].photonPlayer.NickName);
                    return;
                }
            }

            SendResponse("Invalid argument(s)");
        }
        else if(args[0] == "unlock")
        {
            if(args.Length <= 2)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId) && int.TryParse(args[2], out int recipeId))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count && recipeId >= 0 && recipeId < GameManager.Instance.recipeList.Count)
                {
                    photonView.RPC("UnlockRecipeRPC", RpcTarget.AllBuffered, playerId, recipeId);
                    SendResponse("Unlocked " + GameManager.Instance.recipeList[recipeId].name + " recipe for " + GameManager.Instance.playerList[playerId].photonPlayer.NickName);
                    return;
                }
            }

            SendResponse("Invalid argument(s)");
        }
        else if(args[0] == "exp")
        {
            if(args.Length <= 2)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId) && int.TryParse(args[2], out int newValue))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count)
                {
                    photonView.RPC("SetExpRPC", RpcTarget.AllBuffered, playerId, newValue);
                    SendResponse(GameManager.Instance.playerList[playerId].photonPlayer.NickName + " exp = " + GameManager.Instance.playerList[playerId].playerObject.GetComponent<ExperienceSystem>().experience);
                    return;
                }
            }

            SendResponse("Invalid argument(s)");
        }
        else if (args[0] == "setadmin")
        {
            if (args.Length <= 2)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId) && bool.TryParse(args[2], out bool newValue))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count)
                {
                    photonView.RPC("SetAdminRPC", RpcTarget.AllBuffered, playerId, newValue);
                    SendResponse(GameManager.Instance.playerList[playerId].photonPlayer.NickName + " isAdmin = " + newValue);
                    return;
                }
            }

           SendResponse("Invalid argument(s)");
        }
        else if (args[0] == "heal")
        {
            if (args.Length <= 1)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count)
                {
                    photonView.RPC("HealRPC", RpcTarget.AllBuffered, playerId);
                    SendResponse("Healed " + GameManager.Instance.playerList[playerId].photonPlayer.NickName);
                    return;
                }
            }

            SendResponse("Invalid argument(s)");
        }
        else if (args[0] == "unlockall")
        {
            if (args.Length <= 1)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int playerId))
            {
                if (playerId >= 0 && playerId < GameManager.Instance.playerList.Count)
                {
                    photonView.RPC("UnlockAllRecipesRPC", RpcTarget.AllBuffered, playerId);
                    SendResponse("Unlocked all recipes for " + GameManager.Instance.playerList[playerId].photonPlayer.NickName);
                    return;
                }
            }

            SendResponse("Invalid argument(s)");
        }
        else if (args[0] == "playerlist")
        {
            SendResponse("Player list:");
            for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
            {
                SendResponse(i + ": " + GameManager.Instance.playerList[i].photonPlayer.NickName);
            }
        }
        else if (args[0] == "itemlist")
        {
            SendResponse("Item list:");
            for (int i = 0; i < GameManager.Instance.itemList.Count; i++)
            {
                SendResponse(i + ": " + GameManager.Instance.itemList[i].name);
            }
        }
        else if (args[0] == "recipelist")
        {
            SendResponse("Recipe list:");
            for (int i = 0; i < GameManager.Instance.recipeList.Count; i++)
            {
                SendResponse(i + ": " + GameManager.Instance.recipeList[i].product.name);
            }
        }
        else
        {
            SendResponse("Invalid command");
        }
    }

    [PunRPC]
    public void GiveItemRPC(int playerId, int itemId)
    {
        if (GameManager.Instance.playerList[playerId] == Player.myPlayer)
        {
            GameManager.Instance.playerList[playerId].playerObject.GetComponent<InventoryManager>().AddItem(GameManager.Instance.itemList[itemId], GameManager.Instance.itemList[itemId].defaultData);
        }
    }

    [PunRPC]
    public void UnlockAllRecipesRPC(int playerId)
    {
        if (GameManager.Instance.playerList[playerId] == Player.myPlayer)
        {
            GameManager.Instance.playerList[playerId].playerObject.GetComponent<RecipeManager>().UnlockAllRecipes();
        }
    }
    [PunRPC]
    public void UnlockRecipeRPC(int playerId, int recipeId)
    {
        if (GameManager.Instance.playerList[playerId] == Player.myPlayer)
        {
            GameManager.Instance.playerList[playerId].playerObject.GetComponent<RecipeManager>().UnlockRecipe(GameManager.Instance.recipeList[recipeId]);
        }
    }
    
    [PunRPC]
    public void SetExpRPC(int playerId, int newExp)
    {
        if (GameManager.Instance.playerList[playerId] == Player.myPlayer)
        {
            GameManager.Instance.playerList[playerId].playerObject.GetComponent<ExperienceSystem>().ChangeExperience(newExp);
        }
    }

    [PunRPC]
    public void SetAdminRPC(int playerId, bool newValue)
    {
        GameManager.Instance.playerList[playerId].isAdmin = newValue;
        if(GameManager.Instance.playerList[playerId] == Player.myPlayer) ChangeState(false);
    }

    [PunRPC]
    public void HealRPC(int playerId)
    {
        GameManager.Instance.playerList[playerId].playerObject.GetComponent<HealthSystem>().AddHealth(100);
    }

    public void SendResponse(string response)
    {
        Instantiate(responsePrefab, responsesParent).text = response;
    }
}
