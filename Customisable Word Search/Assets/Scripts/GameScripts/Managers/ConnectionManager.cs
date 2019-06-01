using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
	private Ping ping;
	private float waitTime = 5.0f;
	public bool hasConnection;
	private float pingStartTime;
	private string pingAddress = "8.8.8.8";
	[SerializeField]
	private BoolVariable networkConnection;

	[SerializeField]
	private MessageSystem message;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(ping != null)
		{
			if(ping.isDone)
			{
				ping = null;
				hasConnection = true;
				InternetAvailable(true);
			}
			else
			if(Time.time - pingStartTime >= waitTime)
			{
				ping = null;
				hasConnection = false;
				InternetAvailable(false);
			}
		}
	}

	public void StartConnectionCheck()
	{
		InvokeRepeating("CheckConnection", 0.0f, 1.0f);
	}

	public void StopConnectionCheck()
	{
		CancelInvoke("CheckConnection");
	}

	void CheckConnection()
	{
		if(ping == null)
		{
			if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				InternetAvailable(false);
			}
			else
			{
				ping = new Ping(pingAddress);
				pingStartTime = Time.time;
			}
		}
	}

	void InternetAvailable(bool isAvailable)
	{
		if(isAvailable)
		{
			networkConnection.State = true;
		}
		else
		{
			networkConnection.State = false;
			message.SetMessage(1);
		}
	}
}
