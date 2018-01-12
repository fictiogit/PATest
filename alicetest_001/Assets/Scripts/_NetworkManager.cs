using System;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;



public class _NetworkManager : NetworkManager
{

    private static _NetworkManager Instance;
    private _AliceInstanceManager m_AliceInstanceManager;

    private void Awake()
    {
        Instance = this;
    }


    // Use this for initialization
    private void Start()
    {

        if (m_AliceInstanceManager == null)
            m_AliceInstanceManager = _AliceInstanceManager.instance as _AliceInstanceManager;

        if (m_AliceInstanceManager == null)
            m_AliceInstanceManager = GetComponent<_AliceInstanceManager>();

        m_AliceInstanceManager.trackingOnStart = false;

        networkAddress = "127.0.0.1";
        LoadConfig(Utility.Instance.DataPath + "network.xml");
        m_AliceInstanceManager.StartTrack();

    }

    // Update is called once per frame
    public override void OnServerConnect(NetworkConnection conn)// 네트워크가 연결 되었을 때 함수
    {
        Debug.Log("Server connect : " + conn.address + " / Player" + conn.connectionId);
        base.OnServerConnect(conn);
    }
    //--------------------------------------------------------------------------------
    public override void OnServerDisconnect(NetworkConnection conn)// 네트워크가 연결이 끊겼을 때 함수
    {
        Debug.Log("Server Disconnected  : Player" + conn.connectionId + " / " + conn.lastError);
        base.OnServerDisconnect(conn);
    }
    //--------------------------------------------------------------------------------  
    private void LoadConfig(string sFilePath)
    {
        XmlTextReader kReader = null;
        try
        {
            kReader = new XmlTextReader(sFilePath);
            XmlDocument kDocument = new XmlDocument();
            kDocument.Load(kReader);
            XmlNode kRootNode = kDocument.ChildNodes[1];
            if (kRootNode == null)
                return;

            foreach (XmlNode kChildNode in kRootNode.ChildNodes)
            {
                if ("Server" == kChildNode.Name)
                {
                    LoadXML(kChildNode);
                }
                else if ("Mocap" == kChildNode.Name)
                {
                    LoadMocap(kChildNode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            if (kReader != null)
                kReader.Close();
        }
    }
    //--------------------------------------------------------------------------------
    private void LoadXML(XmlNode kNode)//XML 파일을 불러옴
    {
        XmlNode ip = kNode.Attributes.GetNamedItem("ip");
        networkAddress = ip.Value;
        Debug.Log("IP : " + networkAddress);

        XmlNode enable = kNode.Attributes.GetNamedItem("isServer");
        if (enable.Value == "true")
        {
            StartServer();
            Debug.Log("Start Server!");
        }
        else
        {
            StartClient();
            Debug.Log("Start Client!");
        }
    }
    //--------------------------------------------------------------------------------
    private void LoadMocap(XmlNode kNode)
    {
        XmlNode ip = kNode.Attributes.GetNamedItem("ip");
        m_AliceInstanceManager.StreamIP = ip.Value;
        Debug.Log("Mocap IP : " + m_AliceInstanceManager.StreamIP);

        XmlNode port = kNode.Attributes.GetNamedItem("port");
        m_AliceInstanceManager.StreamPort = Int16.Parse(port.Value);
        Debug.Log("Mocap Port : " + m_AliceInstanceManager.StreamPort);
    }
}
