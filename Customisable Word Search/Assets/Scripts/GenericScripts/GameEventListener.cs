using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	public GameEvent Event;
	public UnityEvent Response;

	public static GameEventListener AddGameEventListener(GameObject obj, GameEvent @event)
	{
		obj.SetActive(false);
		GameEventListener listener = obj.AddComponent<GameEventListener>() as GameEventListener;
		listener.Event = @event;
		obj.SetActive(true);
		return listener;
	}

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnregisterListener(this);
	}

	public void OnEventRaised()
	{
		Response.Invoke();
	}


}
