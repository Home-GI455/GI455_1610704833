using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

public class WebSocketConnections : MonoBehaviour
{
   

    //Code for connect server
    private WebSocket websocket;
    public InputField ServerNum;
    public InputField PortNum;

    //Code for chat
    //[SerializeField]
    //List<Message> messagelists = new List<Message>();
    //public GameObject TextObject;
    //public GameObject Chatbox;
    public InputField EnterMesseage_Word_Sentences;
    public string text;
    public Text TextOBJ;
    //public GameObject TextFix;


    public void GotoMessageChat()
    {
        websocket = new WebSocket("ws://127.0.0.1:16592");
        websocket.OnMessage += OnMessage;
        websocket.Connect();
        //websocket.Send("Online");
    }


    private void Start()
    {
        //SendM();
    }
    private void Update()
    {
        //SendM();
    }
    public void SendM()
    {
        text = EnterMesseage_Word_Sentences.text;
        EnterMesseage_Word_Sentences.text = "";
        websocket.Send(text);
        //MessageChat(EnterMesseage_Word_Sentences.text);
        //websocket.Send(TextFix.GetComponent<Text>().text);
        //EnterMesseage_Word_Sentences.ActivateInputField();
        //if (EnterMesseage_Word_Sentences.text != "")
        //{
        //    MessageChat(EnterMesseage_Word_Sentences.text);
        //    websocket.Send(EnterMesseage_Word_Sentences.text);
        //}
        //else
        //{
        //    if (!EnterMesseage_Word_Sentences.isFocused)
        //    {
        //        EnterMesseage_Word_Sentences.ActivateInputField();
        //    }
        //}

        //if (!EnterMesseage_Word_Sentences.isFocused)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        MessageChat("Error not correct");
        //    }
        //}

    }


    private void OnDestroy()
    {
        if(websocket != null)
        {
            websocket.Close();
        }
    }

    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        //Debug.Log("Receive msg" + messageEventArgs.Data);
        //MessageChat(text);
        //TextObject = messageEventArgs.Data;
        //MessageChat();
        //SendM();
        TextOBJ.text += "\n" + messageEventArgs.Data;
        Debug.Log("Receive msg" + messageEventArgs.Data);

    }

    public void MessageChat()
    {

        text = EnterMesseage_Word_Sentences.text;
        EnterMesseage_Word_Sentences.text = "";
        websocket.Send(text);
        //Message newMessage = new Message();
        //newMessage.text = text;
        //messagelists.Add(newMessage);
        //GameObject newText = Instantiate(TextObject, Chatbox.transform);
        //newMessage.TextObject = newText.GetComponent<Text>();
        //newMessage.TextObject.text = newMessage.text;
        //websocket.Send(text);
    }

   
}

//[System.Serializable]
//public class Message
//{
//    public string text;
//    public Text TextObject;
//    //public string TextOBJ;
//}
