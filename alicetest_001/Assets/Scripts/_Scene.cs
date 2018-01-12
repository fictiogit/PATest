using UnityEngine;
using UnityEngine.Networking;

public class _Scene : NetworkBehaviour
{
    public Camera m_MainCamera;
    //--------------------------------------------------------------------------------
    private void Start()
    {
        m_MainCamera.enabled = false;//기본 카메라 끄기 단, 서버에서만 작동가능
        if (isServer)
            m_MainCamera.enabled = true;
    }
    //--------------------------------------------------------------------------------
}
