using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PossibleStatesMiner
{
	Wait=0,GoMine=1,Mining=2,ToGoldBox=3
}
public enum PossibleEventsMiner
{
	GoMine=0,StartMining=1,GoToGoldBox=2
}
public class MinerAi : MonoBehaviour
{
	private const float TimeBetweenRayCastCheck = 3.0f;

	
	public MineManager MineManager;
	public GameObject GoldBox;
	public int MaximunGoldPerCycle;
	public int MoveVelocity;
	public int MineVelocity;
		
	private MinableGoldMine _minableMine;
	private Fsm _finiteStateMachine;
	private PossibleStatesMiner _actualState;
	private int _goldCollected=0;
	private Animation _myAnim;
	private float _timeMining = 0;
	private Vector3 _lastDestination = Vector3.negativeInfinity;
	private List<Vector3> _pathToDestination = new List<Vector3>();
	private PathFindingAgent _agent;
	private RaycastHit _raycastToDestination;
	private CountdownTimer _raycastCheckTimer;
	

	void Awake()
	{
		_myAnim = GetComponent<Animation>();
		_agent = GetComponent<PathFindingAgent>();
	}
	
	void Start () {
		_finiteStateMachine = new Fsm(4,4);
		_finiteStateMachine.SetRelation((int)PossibleStatesMiner.Wait,(int)PossibleEventsMiner.GoMine,(int)PossibleStatesMiner.GoMine);
		
		_finiteStateMachine.SetRelation((int)PossibleStatesMiner.GoMine,(int)PossibleEventsMiner.StartMining,(int)PossibleStatesMiner.Mining);
		
		_finiteStateMachine.SetRelation((int)PossibleStatesMiner.Mining,(int)PossibleEventsMiner.GoToGoldBox,(int)PossibleStatesMiner.ToGoldBox);
		
		_finiteStateMachine.SetRelation((int)PossibleStatesMiner.ToGoldBox,(int)PossibleEventsMiner.GoMine,(int)PossibleStatesMiner.GoMine);
		//_myAnim.Stop();
		_myAnim.Stop();
		_minableMine = MineManager.GetMineWithGold();
		_raycastCheckTimer = new CountdownTimer(TimeBetweenRayCastCheck,TimeBetweenRayCastCheck);
		_finiteStateMachine.SetEvent((int)PossibleEventsMiner.GoMine);
	}
	
	
	void Update ()
	{
		_actualState = (PossibleStatesMiner)_finiteStateMachine.GetState();
		
		
		switch (_actualState)
		{
			case PossibleStatesMiner.Wait:
				break;
			case PossibleStatesMiner.Mining:
				_timeMining += Time.deltaTime;
				if (_minableMine.TimeToMine / MineVelocity < _timeMining)
				{
					_timeMining = 0;
					if (!_minableMine.GetGold())
					{
						_finiteStateMachine.SetEvent((int)PossibleEventsMiner.GoToGoldBox);
						_myAnim.Stop();
					}
					else
					{
						_goldCollected++;
						if (_goldCollected == MaximunGoldPerCycle)
						{
							_finiteStateMachine.SetEvent((int) PossibleEventsMiner.GoToGoldBox);
							_myAnim.Stop();

						}
					}
				}
				break;
			case PossibleStatesMiner.GoMine:
				if (_minableMine != null && _minableMine.AmountOfGold > 0)
					GoToPosition(_minableMine.transform.position);
				else
				{
					if (_minableMine != null)
					{
						_minableMine = MineManager.GetMineWithGold();
					}
				}
				break;
			case PossibleStatesMiner.ToGoldBox:
				//_myAnim.Stop();
				GoToPosition(GoldBox.transform.position);
				//transform.position = Vector3.MoveTowards(transform.position,GoldBox.transform.position,Time.deltaTime*MoveVelocity);
				break;
		}
		
		//Debug.Log(_actualState);
	}

	//TODO: This method can cause problems, if want to use this AI and you have issues, refactor please
	private void GoToPosition(Vector3 destination)
	{
		bool canSeeDestination = false;
		bool destinationHasChange = destination != _lastDestination;
		if (destinationHasChange)
		{
			_raycastCheckTimer = new CountdownTimer(TimeBetweenRayCastCheck,TimeBetweenRayCastCheck);
		}
		_raycastCheckTimer.Update(Time.deltaTime);

		Vector3 directionToMove = destination;
		if (_raycastCheckTimer.CurrentValue > _raycastCheckTimer.MaxValue)
		{
			canSeeDestination = !Physics.Raycast(transform.position, destination);
			_raycastCheckTimer.Reset();
		}

		if (!canSeeDestination)
		{
			if (destinationHasChange)
			{
				_pathToDestination = _agent.GetPathToDestination(destination);
				_lastDestination = destination;
			}
			if(transform.position.x == _pathToDestination[0].x &&
			   transform.position.z == _pathToDestination[0].z)
				_pathToDestination.RemoveAt(0);

			directionToMove = _pathToDestination[0];
		}

		directionToMove.y = destination.y;
		transform.position = Vector3.MoveTowards(transform.position,directionToMove,Time.deltaTime*MoveVelocity);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.name.Equals(GoldBox.gameObject.name))
		{
			_goldCollected = 0;
			_finiteStateMachine.SetEvent((int)PossibleEventsMiner.GoMine);
		}
		
		if (col.name.Equals(_minableMine.gameObject.name))
		{
			_finiteStateMachine.SetEvent((int)PossibleEventsMiner.StartMining);
			_myAnim.Play();

		}


	}
	
}
