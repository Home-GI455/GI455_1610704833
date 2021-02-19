using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocketWithJson
{
    class MessageData
    {
        public string username;
        public string message;
    }
    
    public class WebSocketConnection : MonoBehaviour
    {
        public GameObject rootConnection;
        public GameObject rootMessenger;

        public InputField inputText;
        public InputField inputUsername;
        public Text sendText;
        public Text receiveText;
        
        private WebSocket ws;

        private string tempMessageString;

        void Start()
        {
            rootConnection.SetActive(true);
            rootMessenger.SetActive(false);
        }

        public void Connect()
        {
            string url = $"ws://127.0.0.1:16592/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            rootConnection.SetActive(false);
            rootMessenger.SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        
        public void SendMessage()
        {
            if (inputText.text == "" || ws.ReadyState != WebSocketState.Open)
                return;

            MessageData messageData = new MessageData();
            messageData.username = inputUsername.text;
            messageData.message = inputText.text;

            string toJsonstr = JsonUtility.ToJson(messageData);
            Debug.Log(toJsonstr);

            ws.Send(inputText.text);
            inputText.text = "";
        }

        private void OnDestroy()
        {
            if (ws != null)
                ws.Close();
        }

        private void Update()
        {
            if(string.IsNullOrEmpty(tempMessageString) == false)
            {
                MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);

                if (tempMessageString != null && tempMessageString != "")
                {
                    sendText.text += receiveMessageData.message + "\n";
                }
                else
                {
                    receiveText.text += receiveMessageData.message + "\n";
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempMessageString = messageEventArgs.Data;
        }
    }
}


