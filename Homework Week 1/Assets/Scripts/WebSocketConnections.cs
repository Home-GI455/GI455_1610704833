using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

namespace OnlineChatWithJson
{
   
    public class WebSocketConnections : MonoBehaviour
    {
        struct DataofMessage
        {
            public string OnlineName;
            public string Message;
            public DataofMessage(string OnlineName, string Message)
            {
                this.OnlineName = OnlineName;
                this.Message = Message;
            }

        }

        public struct SocketEvent
        {
            public string eventName;
            public string data; //data

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        
        private WebSocket websocket;

        public GameObject RootConnect;
        public GameObject RootChat;
        public GameObject rootCreateRoomWindow;
        public GameObject rootJoinRoomWindow;
        public GameObject rootLobby;
        public GameObject CreateRoomFail;
        public GameObject JoinRoomFail;
        public GameObject rootLogin;
        public GameObject rootRegister;
        public GameObject rootRorL;
        public GameObject RegisterFail;
        public GameObject LoginFail;
        public GameObject Please_put_information;
        

        public InputField CreateRoomName;
        public InputField JoinRoonName;
        public InputField EnterMesseage_Word_Sentences;
        //public InputField Enter_Onlinename;
        public InputField CreateID;
        public InputField CreatePassword;
        public InputField EnterRepassword;
        public InputField CreateName;
        public InputField Enter_LoginID;
        public InputField Enter_Password;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;
        public DelegateHandle Register;
        public DelegateHandle Login;

        public Text Sendtext;
        public Text ReceiveText;
        public Text Roomname;
        private string TempMessageString;

        //Start window หน้าต่างเริ่มต้น และ เชื่อมเข้า server
        private void Start()
        {
            
            RootConnect.SetActive(true);
            RootChat.SetActive(false);
            rootLobby.SetActive(false);
            rootCreateRoomWindow.SetActive(false);
            CreateRoomFail.SetActive(false);
            JoinRoomFail.SetActive(false);
            rootJoinRoomWindow.SetActive(false);
            rootLogin.SetActive(false);
            rootRegister.SetActive(false);
            rootRorL.SetActive(false);
            RegisterFail.SetActive(false);
            Please_put_information.SetActive(false);
    
        }
        public void Connection()
        {
            string url = "ws://127.0.0.1:16592/";
            websocket = new WebSocket(url);
            websocket.OnMessage += OnMessage;
            
            websocket.Connect();
            
            //SocketEvent socketEvent = new SocketEvent("CreateRoom","JoinRoom");
            //string toJsonStr = JsonUtility.ToJson(socketEvent);
            //websocket.Send(toJsonStr);

            rootRorL.SetActive(true);
            RootConnect.SetActive(false);
        }

        //เปลี่ยนหน้าจอ
        public void GotoRegisterMenu()
        {
            rootRorL.SetActive(false);
            rootRegister.SetActive(true);
        }
        public void GotoLoginMenu()
        {
            rootRorL.SetActive(false);
            rootLogin.SetActive(true);
        }
        public void GotoCreateRoomWindow() 
        {
            rootLobby.SetActive(false);
            rootCreateRoomWindow.SetActive(true);
        }
        public void GotoJoinRoom()
        {
            rootLobby.SetActive(false);
            rootJoinRoomWindow.SetActive(true);
        }
        public void BacktoMenu()
        {
            rootCreateRoomWindow.SetActive(false);
            rootJoinRoomWindow.SetActive(false);
            rootLobby.SetActive(true);
        }
        //Login กับ Register
        public void RegistertoDatabase(string ProfiletoData)
        {
            if (CreateID.text == "" || CreateName.text == ""|| CreatePassword.text == "" ||EnterRepassword.text == "")
            {
                Please_put_information.SetActive(true);
               
            }
            else
            {
                if (CreatePassword.text == EnterRepassword.text)
                {
                    ProfiletoData = CreateID.text + "#" + CreatePassword.text + "#" + CreateName.text;

                    SocketEvent socketEvent = new SocketEvent("Register", ProfiletoData);

                    string toJsonStr = JsonUtility.ToJson(socketEvent);

                    websocket.Send(toJsonStr);
                    Please_put_information.SetActive(false);
                    rootRegister.SetActive(false);
                    rootRorL.SetActive(true);

                }
                else
                {
                    RegisterFail.SetActive(true);
                }
            }
            
            
            //ProfiletoData = CreateID.text + "#" + CreatePassword.text + "#" + CreateName.text;

            //SocketEvent socketEvent = new SocketEvent("Register", ProfiletoData);

            //string toJsonStr = JsonUtility.ToJson(socketEvent);

            //websocket.Send(toJsonStr);



        }
        public void LogintoServer(string ID_Pass)
        {
            
            ID_Pass = Enter_LoginID.text + "#" + Enter_Password.text;
            SocketEvent socketEvent = new SocketEvent("Login", ID_Pass);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);
          

        }
        //สร้างห้อง (Create room), เข้าห้อง (Join Room) และ ออกจากห้อง 
        public void CreateRoom(string RoomName)
        {

           
            RoomName = CreateRoomName.text;

            Roomname.text = RoomName;
            SocketEvent socketEvent = new SocketEvent("CreateRoom", RoomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);

            Please_put_information.SetActive(false);
            //RoomName = CreateRoomName.text;

            //Roomname.text = RoomName;
            //SocketEvent socketEvent = new SocketEvent("CreateRoom", RoomName);

            //string toJsonStr = JsonUtility.ToJson(socketEvent);

            //websocket.Send(toJsonStr);

            //Please_put_information.SetActive(false);

            //RootChat.SetActive(true);
            //rootCreateRoomWindow.SetActive(false);

            //if (CreateRoomName.text == RoomName)
            //{
            //    UpdateNotifyMessage();
            //    RootChat.SetActive(false);
            //    CreateRoomFail.SetActive(true);
            //    OnDestroy();
            //}
        }

        public void JoinRoom(string RoomName)
        {

            
            RoomName = JoinRoonName.text;


            Roomname.text = RoomName;
            SocketEvent socketEvent = new SocketEvent("JoinRoom", RoomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);
            Please_put_information.SetActive(false);


            //RootChat.SetActive(true);
            //rootJoinRoomWindow.SetActive(false);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);

            

        }
        private void Update()
        {
            
            UpdateNotifyMessage();
            if (string.IsNullOrEmpty(TempMessageString) == false)
            {
                DataofMessage receiveMessageData = JsonUtility.FromJson<DataofMessage>(TempMessageString);

                if (TempMessageString != null && TempMessageString != "")
                {
                    Sendtext.text += receiveMessageData.Message + "\n";
                }
                else
                {
                    ReceiveText.text += receiveMessageData.Message + "\n";
                }

                TempMessageString = "";
            }
        }

        //ส่งข้อความไปยัง server และ ระบบแชท
        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(TempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(TempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                    if (receiveMessageData.data != "fail") 
                    {
                        rootCreateRoomWindow.SetActive(false);
                        RootChat.SetActive(true);
                    }
                    else
                    {
                        CreateRoomFail.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                    if (receiveMessageData.data != "fail")
                    {
                        rootJoinRoomWindow.SetActive(false);
                        RootChat.SetActive(true);
                        
                    }
                    else
                    {
                        JoinRoomFail.SetActive(true);
                        
                    }
                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                    RootChat.SetActive(false);
                    rootLobby.SetActive(true);
                }
                else if(receiveMessageData.eventName == "Register")
                {
                    if (Register != null)
                        Register(receiveMessageData);
                    if(receiveMessageData.data != "Fail")
                    {
                        rootRorL.SetActive(true);
                        rootRegister.SetActive(false);
                        RegisterFail.SetActive(false);
                        Debug.Log("success");
                    }
                    else
                    {
                        RegisterFail.SetActive(true);
                        Debug.Log("Fail");
                    }
                }
                else if(receiveMessageData.eventName == "Login")
                {
                    if (Login != null)
                        Login(receiveMessageData);
                    if(receiveMessageData.data != "Fail")
                    {
                        rootLogin.SetActive(false);
                        rootLobby.SetActive(true);
                        LoginFail.SetActive(false);
                        Please_put_information.SetActive(false);
                    }
                    else
                    {
                        LoginFail.SetActive(true);
                        Please_put_information.SetActive(true);
                    }
                }

                TempMessageString = "";
            }

            
        }
        public void Disconnect()
        {
            if (websocket != null)
                websocket.Close();
        }
        public void SendM()
        {
            if (EnterMesseage_Word_Sentences.text == "" || websocket.ReadyState != WebSocketState.Open)
                return;

            DataofMessage messageData = new DataofMessage();
            //messageData.OnlineName = Enter_Onlinename.text;
            messageData.Message = EnterMesseage_Word_Sentences.text;
            string toJsonstr = JsonUtility.ToJson(messageData);
            Debug.Log(toJsonstr);

            websocket.Send(EnterMesseage_Word_Sentences.text);
            EnterMesseage_Word_Sentences.text = "";
        }


        private void OnDestroy()
        {
            Disconnect();
        }

        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {

            Debug.Log(messageEventArgs.Data);
            TempMessageString = messageEventArgs.Data;

        }
        


    }
}


