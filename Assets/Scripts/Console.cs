using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Console : MonoBehaviourPunCallbacks
{
    public static Console Instance;
    private void Awake() { Instance = this; }

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform responsesParent;
    [SerializeField] private TextMeshProUGUI responsePrefab;
    [SerializeField] private Transform content;

    private TextMeshProUGUI lastResponse;

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        if(!Player.myPlayer.isAdmin) return;

        if (Input.GetKeyDown(KeyCode.BackQuote)) 
        {
            UiManager.Instance.ChangeCurrentPanel(Panels.Console);
        }
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
            SendResponse("unlockall [player_id]");
            SendResponse("unlock [player_id] [recipe_id]");
            SendResponse("time [1-24]");
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
                    photonView.RPC("GiveItemRPC", GameManager.Instance.playerList[playerId].photonPlayer, itemId);
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
                    photonView.RPC("UnlockRecipeRPC", GameManager.Instance.playerList[playerId].photonPlayer, recipeId);
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
                    photonView.RPC("SetExpRPC", GameManager.Instance.playerList[playerId].photonPlayer, newValue);
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
                    photonView.RPC("SetAdminRPC", GameManager.Instance.playerList[playerId].photonPlayer, newValue);
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
                    photonView.RPC("HealRPC", GameManager.Instance.playerList[playerId].photonPlayer);
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
                    photonView.RPC("UnlockAllRecipesRPC", GameManager.Instance.playerList[playerId].photonPlayer);
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
        else if (args[0] == "time")
        {
            if (args.Length <= 1)
            {
                SendResponse("Empty argument(s)");
                return;
            }

            if (int.TryParse(args[1], out int time))
            {
                TimeManager.Instance.SetTime(time);
                SendResponse("Time changed to " + time);
                return;
            }

            SendResponse("Invalid argument(s)");
        }
        else
        {
            SendResponse("Invalid command");
        }
    }

    [PunRPC]
    public void GiveItemRPC(int itemId)
    {
        InventoryManager.Instance.AddItem(GameManager.Instance.itemList[itemId], GameManager.Instance.itemList[itemId].defaultData);
    }

    [PunRPC]
    public void UnlockAllRecipesRPC()
    {
        RecipeManager.Instance.UnlockAllRecipes();
    }
    [PunRPC]
    public void UnlockRecipeRPC(int recipeId)
    {
        RecipeManager.Instance.UnlockRecipe(GameManager.Instance.recipeList[recipeId]);
    }
    
    [PunRPC]
    public void SetExpRPC(int newExp)
    {
        ExperienceSystem.Instance.ChangeExperience(newExp);
    }

    [PunRPC]
    public void SetAdminRPC(bool newValue)
    {
        Player.myPlayer.isAdmin = newValue;
        UiManager.Instance.ChangeCurrentPanel(Panels.None);
    }

    [PunRPC]
    public void HealRPC()
    {
        HealthSystem.Instance.AddHealth(100);
    }

    public void SendResponse(string response)
    {
        lastResponse = Instantiate(responsePrefab, responsesParent);
        lastResponse.text = response;
    }

    public void ScrollToBottomButton()
    {
        if (lastResponse == null) return;
        content.localPosition = new Vector3(0, -lastResponse.transform.localPosition.y - 150, 0);
    }
}
