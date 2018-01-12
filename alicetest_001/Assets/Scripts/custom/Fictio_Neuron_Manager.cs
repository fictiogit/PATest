//█████████n█o█t█e████████
//████████████████████████

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Fictio_Neuron_Manager : NetworkBehaviour // gets all functionalities from network behaviour
{
    [HideInInspector]
    public List<GameObject> hmd_list;//HMD 리스트를 가져옵니다.
    public List<Transform> neuron_list;//Neruon의 리스트를 입력 받습니다.
    public Dropdown ui_player_choice;//Dropdown 메뉴를 선택합니다
    public Slider neuron_scale;//뉴런의 크기를 조절합니다

    public Slider neuron_pos_x;
    public Slider neuron_pos_y;
    public Slider neuron_pos_z;
   
    int input_player_num = 0;//선택된 플레이어를 나타냅니다
    bool is_changeable = false;//Dropdown에서 선택되지 않은 상태에서는 값 변화를 적용하지 않게 합니다.

    [SyncVar]
    public float scale_sync_var = 0;//크기를 맞춰주는 
    [SyncVar]
    public float posX_sync_var = 0;
    [SyncVar]
    public float posY_sync_var = 0;
    [SyncVar]
    public float posZ_sync_var = 0;

    [SyncVar]
    bool is_button_on = false;
    
    void Start()
    {
        //█████████n█o█t█e████████when app starts, get the name of object and save it into string.
        //████████████████████████Add active HMD to HMD list


        string objName = System.String.Format("NeuronRobot_{0:0}", 10000); // applies second value into {0:0} for value  
        hmd_list.AddRange(GameObject.Find("Alice").GetComponent<vrHMDManager>().m_Hmds);//hmd_list 변수에 기존에 입력받은 리스트를 가져옵니다.
    }
    // Update is called once per frame
    void Update()
    {
        //█████████n█o█t█e████████ if it's server side, 
        //████████████████████████

        if (isServer)//서버일 경우 클라이언트는 UI를 볼 수도없고 조절할 권한도 없음
        {
            for(int i= 0; i<neuron_list.Count;i++)//뉴런의 위치값을 재조정한다.
            {
                RpcSyncFreeze_Neuron_Position(i);
                Freeze_Neuron_Position(i);
            }

            //█████████n█o█t█e████████ syncs the dials getting moved for position
            //████████████████████████ and moves it to a separate var to send value to client
            posX_sync_var = neuron_pos_x.value;
            posY_sync_var = neuron_pos_y.value;
            posZ_sync_var = neuron_pos_z.value;

            scale_sync_var = neuron_scale.value;
            //기본적으로 Dropdown은 list입니다. 
            if (ui_player_choice.value == 0) //Dropdown의 위치가 0번째( 가장 처음 위치한 )일 경우
            {
                is_changeable = true;// 뉴런값 변화를 막는 bool 값을 불러옵니다.
            }
            else
            {
                is_changeable = false;// 뉴런값 변화를 적용하는 bool 값을 불러옵니다.
            }
            if (ui_player_choice.value == 1)//1번 플레이어가 선택될 경우
            {
                RpcSyncInputPlayerNum(0);//클라이언트에 전송할 데이터 하나
                InputPlayerNum(0);// 서버에 전송할 데이터 하나
            }
            else if(ui_player_choice.value != 1)
            { 
                RpcSyncReturnPlayerNum(0);
                ReturnPlayerNum(0);
            }   
            if (ui_player_choice.value == 2)
            {
                RpcSyncInputPlayerNum(1);
                InputPlayerNum(1);
            }
            else if (ui_player_choice.value != 2)
            {
                RpcSyncReturnPlayerNum(1);
                ReturnPlayerNum(1);
            }
            if (Input.GetKey(KeyCode.Q)&& !is_changeable)//Q버튼을 누르면 크기 변경이 실행된다. 
            {
                RpcSyncLocal_Scale();
                Local_Scale();
            }
        }    }
    //-----------------------------------------------------------------------------------------------------
    [ClientRpc] //sends data to client
    private void RpcSyncInputPlayerNum(int a)//클라이언트에 전송
    {
        input_player_num = a;//선택된 변수값을 입력 합니다. 이 변수값은 리스트의 자리값을 불러옵니다.
        hmd_list[a].transform.GetChild(2).GetComponent<MeshRenderer>().material.color = Color.red;// HMD 머리 위 색을 빨간색으로 변경합니다
    }
    private void InputPlayerNum(int a)//나 자신(서버)에 하나
    {
        input_player_num = a;//
        hmd_list[a].transform.GetChild(2).GetComponent<MeshRenderer>().material.color = Color.red;
    }
    //-----------------------------------------------------------------------------------------------------
    [ClientRpc]
    private void RpcSyncReturnPlayerNum(int a)//선택 취소
    {
        hmd_list[a].transform.GetChild(2).GetComponent<MeshRenderer>().material.color = Color.white;
    }
    private void ReturnPlayerNum(int a)
    {
        hmd_list[a].transform.GetChild(2).GetComponent<MeshRenderer>().material.color = Color.white;
    }
    //-----------------------------------------------------------------------------------------------------
    [ClientRpc]//서버에서 호출되어 클라이언트에서 실행
    private void RpcSyncLocal_Scale()
    {
        float a = hmd_list[input_player_num].transform.position.y * scale_sync_var;
        neuron_list[input_player_num].transform.localScale = new Vector3(a, a ,a);
    }   
    private void Local_Scale()
    {
        float a = hmd_list[input_player_num].transform.position.y * scale_sync_var;
        neuron_list[input_player_num].transform.localScale = new Vector3(a, a, a);
    }

    //-----------------------------------------------------------------------------------------------------
    [ClientRpc]//클라이언트에만 적용! 서버에는 미적용
    private void RpcSyncFreeze_Neuron_Position(int i)
    {
        neuron_list[i].transform.position = new Vector3(posX_sync_var + hmd_list[i].transform.GetChild(1).transform.position.x, posY_sync_var + 0.5f, hmd_list[i].transform.GetChild(1).transform.position.z - 0.106f/*offset value*/ + posZ_sync_var);
    }
    private void Freeze_Neuron_Position(int i)
    {
        neuron_list[i].transform.position = new Vector3(posX_sync_var + hmd_list[i].transform.GetChild(1).transform.position.x, posY_sync_var +0.5f, hmd_list[i].transform.GetChild(1).transform.position.z - 0.106f + posZ_sync_var);
    }
    //-----------------------------------------------------------------------------------------------------

    [ClientRpc]
    public void RpcSync_Scale_Save() // 저장 버튼과 연동 
    {
        PlayerPrefs.SetFloat("Save_Scale", scale_sync_var);
        print(PlayerPrefs.GetFloat("Save_Scale"));
    }
    public void Scale_Save()
    {
        PlayerPrefs.SetFloat("Save_Scale", scale_sync_var);//Save_Scale 이라는 이름으로 저장
    }
}