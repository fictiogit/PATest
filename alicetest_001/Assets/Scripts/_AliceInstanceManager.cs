using ProjectAlice;
using System.Net;

public class _AliceInstanceManager : AliceInstanceManager {

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	public void StartTrack () {

        //checks the network
        StartTrack(new IPEndPoint(IPAddress.Parse(StreamIP), StreamPort), AliceClient.SessionType.DataReader);
	}
}
