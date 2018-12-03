using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferDialog : MonoBehaviour {

	// Use this for initialization
	public GameObject crew1;
	public GameObject crew2;
	public GameObject supply1;
	public GameObject supply2;
	public GameObject fuel1;
	public GameObject fuel2;

	public GameObject crew1Max;
	public GameObject crew2Max;
	public GameObject supply1Max;
	public GameObject supply2Max;
	public GameObject fuel1Max;
	public GameObject fuel2Max;
	public GameObject name1;
	public GameObject name2;

	public Ship ship1;
	public Ship ship2;
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void setShip1(Ship ship) 
	{
		ship1 = ship;
		name1.GetComponent<Text>().text = ship.Name;
		setCrew1(ship1.Crew);
		setSupply1(ship1.Supply);
		setFuel1(ship1.Fuel);		
		setCrew1Max(ship1.MaxCrew);
		setSupply1Max(ship1.MaxSupply);
		setFuel1Max(ship1.MaxFuel);			
	}

	public void setShip2(Ship ship) 
	{
		ship2 = ship;
		name2.GetComponent<Text>().text = ship.Name;
		setCrew2(ship2.Crew);
		setSupply2(ship2.Supply);
		setFuel2(ship2.Fuel);	
		setCrew2Max(ship2.MaxCrew);
		setSupply2Max(ship2.MaxSupply);
		setFuel2Max(ship2.MaxFuel);				
	}

	public void refresh() 
	{
		setCrew1(ship1.Crew);
		setSupply1(ship1.Supply);
		setFuel1(ship1.Fuel);	
		setCrew2(ship2.Crew);
		setSupply2(ship2.Supply);
		setFuel2(ship2.Fuel);	
		setCrew2Max(ship2.MaxCrew);
		setSupply2Max(ship2.MaxSupply);
		setFuel2Max(ship2.MaxFuel);		
		setCrew1Max(ship1.MaxCrew);
		setSupply1Max(ship1.MaxSupply);
		setFuel1Max(ship1.MaxFuel);							
	}

	public void send1Crew2() 
	{
		int amount = 1;
		Ship.Transfer(ship2, ship1, Resource.Crew, ref amount);
		refresh();
	}

	public void send2Crew1() 
	{
		int amount = 1;
		Ship.Transfer(ship1, ship2, Resource.Crew, ref amount);
		refresh();
	}

	public void send1Supply2() 
	{
		int amount = 1;
		Ship.Transfer(ship2, ship1, Resource.Supply, ref amount);
		refresh();
	}

	public void send2Supply1()
	{
		int amount = 1;
		Ship.Transfer(ship1, ship2, Resource.Supply, ref amount);
		refresh();
	}	
	public void send1Fuel2() 
	{
		int amount = 1;
		Ship.Transfer(ship2, ship1, Resource.Fuel, ref amount);
		refresh();
	}

	public void send2Fuel1() 
	{
		int amount = 1;
		Ship.Transfer(ship1, ship2, Resource.Fuel, ref amount);
		refresh();
	}	
	public void close() 
	{
		DialogManager.CloseTransferDialog();
		
		// TODO: This just closes the dialog
	}

	public void setCrew1(int count) 
	{
		crew1.GetComponent<Text>().text = "" + count;
	}

	public void setCrew2(int count) 
	{
		crew2.GetComponent<Text>().text = "" + count;
	}

	public void setSupply1(int count) 
	{
		supply1.GetComponent<Text>().text = "" + count;
	}

	public void setSupply2(int count) 
	{
		supply2.GetComponent<Text>().text = "" + count;
	}

	public void setFuel1(int count)
	{
		fuel1.GetComponent<Text>().text = "" + count;
	}
	public void setFuel2(int count) 
	{
		fuel2.GetComponent<Text>().text = "" + count;
	}	

	public int getCrew1() 
	{
		return int.Parse(crew1.GetComponent<Text>().text);
	}

	public int getCrew2() 
	{
		return int.Parse(crew2.GetComponent<Text>().text);
	}

	public int getSupply1() 
	{
		return int.Parse(supply1.GetComponent<Text>().text);
	}

	public int getSupply2() 
	{
		return int.Parse(supply2.GetComponent<Text>().text);
	}

	public int getFuel1() 
	{
		return int.Parse(fuel1.GetComponent<Text>().text);
	}
	public int getFuel2() {
		return int.Parse(fuel2.GetComponent<Text>().text);
	}	
		
	public void setCrew1Max(int count) 
	{
		crew1Max.GetComponent<Text>().text = "/" + count;
	}

	public void setCrew2Max(int count) 
	{
		crew2Max.GetComponent<Text>().text = "/" + count;
	}

	public void setSupply1Max(int count) 
	{
		supply1Max.GetComponent<Text>().text = "/" + count;
	}

	public void setSupply2Max(int count) 
	{
		supply2Max.GetComponent<Text>().text = "/" + count;
	}

	public void setFuel1Max(int count)
	{
		fuel1Max.GetComponent<Text>().text = "/" + count;
	}
	public void setFuel2Max(int count) 
	{
		fuel2Max.GetComponent<Text>().text = "/" + count;
	}			
}
