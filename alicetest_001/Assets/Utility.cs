using UnityEngine;

public class Utility : MonoBehaviour//XXML 파일의 위치를 읽어옵니다
{
    public static Utility Instance;

    private string m_sAppPath = string.Empty;
    private string m_sDataPath = string.Empty;
    //--------------------------------------------------------------------------------
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        SetPath();
    }
    //--------------------------------------------------------------------------------
    public string AppPath
    {
        get { return this.m_sAppPath; }
        set { this.m_sAppPath = value; }
    }
    //--------------------------------------------------------------------------------
    public string DataPath
    {
        get { return this.m_sDataPath; }
        set { this.m_sDataPath = value; }
    }
    //--------------------------------------------------------------------------------
    private void SetPath()
    {
        //
        int islashIndex = Application.dataPath.LastIndexOf("/");
        AppPath = Application.dataPath.Substring(0, islashIndex);

        if (Application.dataPath.Contains("Assets"))
        {
            DataPath = AppPath + "/Bin/Data/";
        }
        else
        {
            DataPath = AppPath + "/Data/";
        }
    }
    //--------------------------------------------------------------------------------
}