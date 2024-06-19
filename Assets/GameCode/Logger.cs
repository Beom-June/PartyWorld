using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2��
 * Logger Ŭ������ �����ڰ� ������ ������ �� �ֿܼ��� �޽����� Ȯ���ϰ� ������� �� ���˴ϴ�. 
 * ���� ������ ���������� Ư�� �α� ������ ����Ͽ� �ʿ��� �α� �޽����� ����� �� �ֽ��ϴ�. 
 * ���� Logger Ŭ������ Unity���� ������ �����ϰ� ������ϴ� �� �ʼ����Դϴ�.
 * 
 * 3��
 * ���� : ���� ���� �α׸� ����ϴ� ��� ���� ����
 * 
 */
public class Logger : MonoBehaviour
{
    public string output = "";
    public string stack = "";

    //  �α� �̺�Ʈ Ŭ����
    public class LogEvent
    {
        public string _message;
        public string _trace;
        public LogType _type;

        public LogEvent(string _message, string _trace, LogType _type)
        {
            this._message = _message;
            this._trace = _trace;
            this._type = _type;
        }
    }
    private void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Input Key 'A' Log");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.LogException(new System.Exception("Input Key 'S' Exception"));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.LogError("Input Key 'D' Error");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Assert(false, "Input Key 'F' Assert");
        }
    }
    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        Debug.Log(type + "\ncondition => " + condition + "\nstackTrace => " + stackTrace);
    }
}
