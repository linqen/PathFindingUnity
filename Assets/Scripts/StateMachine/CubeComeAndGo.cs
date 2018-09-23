using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleStates
{
	Wait=0,Come=1,Go=2,
}
public enum PossibleEvents
{
	ToggleStartWait=0,Arrive=1
}

public class CubeComeAndGo : MonoBehaviour
{
	public GameObject DestinationGo;
	public GameObject DestinationCome;
	public float MoveVelocity=1;
	
	private Fsm _finiteStateMachine;
	private PossibleStates _actualState;
	
	void Start () {
		_finiteStateMachine = new Fsm(3,2);
		_finiteStateMachine.SetRelation((int)PossibleStates.Wait,(int)PossibleEvents.ToggleStartWait,(int)PossibleStates.Go);
		_finiteStateMachine.SetRelation((int)PossibleStates.Go,(int)PossibleEvents.Arrive,(int)PossibleStates.Come);
		_finiteStateMachine.SetRelation((int)PossibleStates.Come,(int)PossibleEvents.Arrive,(int)PossibleStates.Go);
		
		_finiteStateMachine.SetRelation((int)PossibleStates.Go,(int)PossibleEvents.ToggleStartWait,(int)PossibleStates.Wait);
		_finiteStateMachine.SetRelation((int)PossibleStates.Come,(int)PossibleEvents.ToggleStartWait,(int)PossibleStates.Wait);
		
		_finiteStateMachine.SetEvent((int)PossibleEvents.ToggleStartWait);
	}
	
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_finiteStateMachine.SetEvent((int)PossibleEvents.ToggleStartWait);
		}
		_actualState = (PossibleStates)_finiteStateMachine.GetState();
		
		switch (_actualState)
		{
				case PossibleStates.Wait:
					break;
				case PossibleStates.Come:
					transform.position = Vector3.MoveTowards(transform.position,DestinationGo.transform.position,Time.deltaTime*MoveVelocity);
					break;
				case PossibleStates.Go:
					transform.position = Vector3.MoveTowards(transform.position,DestinationCome.transform.position,Time.deltaTime*MoveVelocity);
					break;
		}
		
		
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log(col.name);
		if (col.name.Equals("DestinationCome") || col.name.Equals("DestinationGo"))
		{
			_finiteStateMachine.SetEvent((int)PossibleEvents.Arrive);
		}
	}
}
