using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager: MonoBehaviour
{
    static GameObject transferDialog;
    static GameObject sacrificeShipMsg;
    public static void TransferDialog(Ship transferShip1, Ship transferShip2 ) 
    {

        transferDialog = (GameObject) Instantiate(Resources.Load("prefab/TransferDialog"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        transferDialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        transferDialog.transform.localPosition =  new Vector3(0f, 0f, 0f);
        transferDialog.transform.localScale = new Vector3(1f, 1f, 1f);
        

        // yeah, yeah, it is kinda backwards.  Ship2 is the first ship
        transferDialog.GetComponent<TransferDialog>().setShip1(transferShip2);

        // annd.. ship 1 is the 2nd ship selected.
        transferDialog.GetComponent<TransferDialog>().setShip2(transferShip1);        
        
    }
    public static void CloseTransferDialog() 
    {
        if(GameStateManager.Instance.tutorialOn)
        {
            GameStateManager.Instance.tutorial.TeachRepair();
        }
        Debug.Log("transfer dialog close");
        Destroy(transferDialog);
    }  

    public static void DisplayMessage(string msg) {
        GameObject dialog = (GameObject) Instantiate(Resources.Load("prefab/StatusDialog"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        dialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        dialog.transform.localPosition =  new Vector3(0f, 0f, 0f);
        dialog.transform.localScale = new Vector3(1f, 1f, 1f);

        // dialog.GetComponent<DialogTrigger>()

        dialog.GetComponentInChildren<Text>().text = msg;
    }

    public static void SacrificeShipMessage(bool turnOn) {
        if (sacrificeShipMsg == null) 
        {
            sacrificeShipMsg = GameObject.Find("SacrificeShipMsg");
        }
        sacrificeShipMsg.GetComponent<Text>().enabled = turnOn;
    }



    public static void ScoutingReturnedMessage(FleetManager.ScoutingFinds ship) 
    {
        
        // show dialog
        // ship.supplyFound
        string whatFound = "nothing";
        if (ship.supplyFound > 0) {
            whatFound = "" + ship.supplyFound + " supplies";
        }
        else if (ship.fuelFound > 0) {
            whatFound = "" + ship.fuelFound + " fuel pods";
        }
        else if (ship.shipsFound > 0) {
            whatFound = "" + ship.shipsFound + " ships";
        }
        DialogManager.DisplayMessage("Scouting mission returned with " + whatFound);        
    }      
}