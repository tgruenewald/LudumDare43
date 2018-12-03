using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager: MonoBehaviour
{
    static GameObject transferDialog;
    static GameObject sacrificeShipMsg;

    static GameObject dialog;
    static GameObject shipSpecDialog;

    static Ship shipToBeDestroyed;
    static List<GameObject> dialogList = new List<GameObject>();

    static bool showSpec = true;

    public static void AddDialog(GameObject addedDialog)
    {
        dialogList.Add(addedDialog);
    }
    public static void CloseAllDialogs()
    {
        Debug.Log("Closing all dialogs");
        showSpec = true;
        foreach (GameObject d in dialogList)
        {
            if (d == null)
            {
                //dialogList.Remove(d);
            }
            else
            {
                Destroy(d);
              //  dialogList.Remove(d);
            }
        }
        dialogList.RemoveAll(x => x == null);
    }

    public static void TransferDialog(Ship transferShip1, Ship transferShip2 ) 
    {
        if (dialog != null)
        {
            Destroy(dialog);
        }
        transferDialog = (GameObject) Instantiate(Resources.Load("prefab/TransferDialog"),new Vector3(270f, 100f, 0f), Quaternion.identity); //GameObject.Find("TransferDialog");
        AddDialog(transferDialog);
        transferDialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        transferDialog.transform.localPosition =  new Vector3(270f, 100f, 0f);
        transferDialog.transform.localScale = new Vector3(1f, 1f, 1f);
        

        // yeah, yeah, it is kinda backwards.  Ship2 is the first ship
        transferDialog.GetComponent<TransferDialog>().setShip1(transferShip2);

        // annd.. ship 1 is the 2nd ship selected.
        transferDialog.GetComponent<TransferDialog>().setShip2(transferShip1);        
        
    }
    public static void CloseTransferDialog() 
    {
        if (GameStateManager.Instance.tutorialOn)
            GameStateManager.Instance.tutorial.TeachRepair();
        Debug.Log("transfer dialog close");
        Destroy(transferDialog);
    }  

    public static void hideShipSpec()
    {
        showSpec = true;
        if (shipSpecDialog != null) 
        {
            Destroy(shipSpecDialog);
        }        
    }
    public static void showShipSpec(Ship ship) 
    {
        if (!showSpec) {
            return;
        }
        showSpec = false;

        if (shipSpecDialog != null) 
        {
            Destroy(shipSpecDialog);
        }
        if (shipToBeDestroyed != null) {
            return; // don't display specs until ship is chosen
        }
        AudioManager.Instance.PlaySound(AudioClips.Click);
        shipSpecDialog = (GameObject) Instantiate(Resources.Load("prefab/"+ship.GetType().Name+"Spec"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        AddDialog(shipSpecDialog);
        shipSpecDialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        shipSpecDialog.transform.localPosition =  new Vector3(270f, 100f, 0f);
        shipSpecDialog.transform.localScale = new Vector3(1f, 1f, 1f);

        shipSpecDialog.transform.Find("ShipName").GetComponent<Text>().text = ship.Name;
        shipSpecDialog.transform.Find("Crew").GetComponent<Text>().text = "Crew: " + ship.Crew + "/" + ship.MaxCrew;
        shipSpecDialog.transform.Find("Supply").GetComponent<Text>().text = "Supply: " + ship.Supply + "/" + ship.MaxSupply;
        shipSpecDialog.transform.Find("Fuel").GetComponent<Text>().text = "Fuel: " + ship.Fuel + "/" + ship.MaxFuel;
    }

    public static void SacrificeShip()
    {
        if (shipToBeDestroyed != null)
        {
            shipToBeDestroyed.SacrificeShip();
            shipToBeDestroyed = null;
        }

    }

    public static void AreYouSureMessage(Ship ship) {
        AudioManager.Instance.PlaySound(AudioClips.Click);
        if (shipSpecDialog != null) 
        {
            Destroy(shipSpecDialog);
        }        
        dialog = (GameObject) Instantiate(Resources.Load("prefab/AreYouSure"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        dialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        dialog.transform.localPosition =  new Vector3(270f, 100f, 0f);
        dialog.transform.localScale = new Vector3(1f, 1f, 1f);

        shipToBeDestroyed = ship;
    }    

    public static void DisplayMessage(string msg) {
        AudioManager.Instance.PlaySound(AudioClips.Click);
        dialog = (GameObject) Instantiate(Resources.Load("prefab/StatusDialog"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        AddDialog(dialog);
        dialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        dialog.transform.localPosition =  new Vector3(270f, 100f, 0f);
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
        DialogManager.DisplayMessage(ship.ship.Name + " returned with " + whatFound);        
    }      
}