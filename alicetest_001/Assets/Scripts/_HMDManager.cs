using ProjectAlice;
using System.Collections.Generic;
using UnityEngine;

public class vrHMDManager : MonoBehaviour//HMD를 불러와서 컨트롤하는 스크립트
{
    public List<GameObject> m_Hmds;//m_Hmds의 리스트값을 받아온다
    private bool m_bSetup = false;
    //--------------------------------------------------------------------------------
    private void Start()
    {
        int iSize = m_Hmds.Count;//리스트의 갯수를 계산
        for (int i = 0; i < iSize; ++i)
        {
            AliceHMD hmd = m_Hmds[i].GetComponent<AliceHMD>();
            hmd.ResetCamera();//hmd의 카메라값을 초기화한다
            hmd.SetActiveHMD(false);//hmd의 기능을 일시 중지한다 . 이후 카메라는 설정값과 동일한 이름을 가진 pc를 계산하여 출력한다.
        }
    }
    //--------------------------------------------------------------------------------
    private void SetupHMD()
    {
        int iSize = AliceDeviceConfigs.Instance.configs.Count;//엘리스 디바이스 갯수를 불러온다
        for (int i = 0; i < iSize; ++i)
        {
            AliceDeviceCfg config = AliceDeviceConfigs.Instance.configs[i];//엘리스 디바이스 설정값 리스트를 불러옴
            if (config.IsHmd() && !config.ClientIP.StartsWith("0.0"))
            {
                int jSize = m_Hmds.Count;
                for (int j = 0; j < jSize; ++j)
                {
                    string sHMDName = m_Hmds[j].name;
                    if (config.DeviceName.Equals(sHMDName) || config.RigidBodyName.Equals(sHMDName))//config 디바이스 이름이 sHmdName이랑 같거나 강체 이름이 sHMDName이랑 같으면
                    {
                        AliceHMD hmd = m_Hmds[j].GetComponent<AliceHMD>();
                        hmd.deviceCode = config.DeviceCode;
                        if (config.IsLocal)//로컬 클라이언트랑 일치하면
                        {
                            hmd.SetActiveHMD(true);//활성화
                        }
                        else
                        {
                            hmd.SetActiveHMD(false);//비활성화
                        }
                    }
                }
            }
        }
    }
    //--------------------------------------------------------------------------------
    private void Update()
    {
        if (!AliceDeviceConfigs.Instance.IsDataReceived || m_bSetup)
            return;

        m_bSetup = true;
        SetupHMD();
    }
    //--------------------------------------------------------------------------------
    public void ResetCamera()
    {
        int iSize = m_Hmds.Count;
        for (int i = 0; i < iSize; ++i)
        {
            AliceHMD hmd = m_Hmds[i].GetComponent<AliceHMD>();
            hmd.ResetCamera();
        }
    }
    //--------------------------------------------------------------------------------
}