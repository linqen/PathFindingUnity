public class Fsm{

	private int[,] _fsm;
	private int _state;

	public Fsm(int statesCount, int eventsCount){
		_fsm = new int[statesCount,eventsCount];
		for (int i = 0; i < statesCount; i++)
		{
			for (int j = 0; j < eventsCount; j++)
			{
				_fsm[i, j] = -1;
			}
		}
	}

	public void SetRelation(int srcState, int evt, int destinationState){
		_fsm[srcState,evt] = destinationState;
	}
	
	public int GetState(){
		return _state;
	}
	
	public void SetEvent(int evt){
		if(_fsm[_state,evt] !=-1){
			_state = _fsm[_state,evt];
		}
	}
}
