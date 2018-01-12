using UnityEngine;
using System.Collections;

namespace ProjectAlice
{
    public class ConfigDataReceiver : MonoBehaviour {


        public AliceHMD hmd;
        public AliceRigidbody rigidBody;
        public ObjectControlled controller;

        bool hasPrinted = false;
        // Update is called once per frame
        void Update()
        {
            if(AliceDeviceConfigs.Instance.IsDataReceived && !hasPrinted)
            {
                Debug.Log("got AliceDeviceConfigs");
                var myHmd = AliceDeviceConfigs.Instance.GetMineHmdConfig();
                var myWii = AliceDeviceConfigs.Instance.GetMineWiiConfig();
                if(myHmd != null)
                {
                    Debug.Log("got myHmd : " + myHmd.ToString());
                    hmd.deviceCode = myHmd.DeviceCode;
                }
                if (myWii != null)
                {
                    Debug.Log("got myWii : " + myWii.ToString());
                    rigidBody.deviceCode = myWii.DeviceCode;
                    controller.wiiID = myWii.ControllerCode;
                }
                hasPrinted = true;
            }
        }
    }
}
