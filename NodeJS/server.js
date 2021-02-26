const app = require('express')();
const server = require('http').Server(app);
const websocket = require('ws');
const wss = new websocket.Server({server});
const sqlite3 = require('sqlite3').verbose();

server.listen(process.env.PORT || 16592, ()=>{
    console.log("Server start at port "+server.address().port);
});


/*
{
    roomName: ""
    wsList: []
}
*/

var db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>
{
    if(err) throw err;

    console.log("Connect to Database");
    
    var wsList = [];
    var roomList = [];
    wss.on("connection", (ws)=>{    

    //Lobby
    console.log("client connected.");
    //Reception
    ws.on("message", (data)=>{
        console.log("send from client :"+ data);
        
        var toJsonObj = {
            eventName:"",
            data:"test#111111",
            username:""
        }
        //===============================================
        toJsonObj = JSON.parse(data);
        //===============================================

        // SaveData
        var splitStr = toJsonObj.data.split('#');

        var userID = splitStr[0];
        var password = splitStr[1];
        var name = splitStr[2];

        //========== Convert jsonStr into jsonObj =======

        //toJsonObj = JSON.parse(data);

        // I change to line below for prevent confusion
        
        
        //===============================================

        if(toJsonObj.eventName == "CreateRoom")//CreateRoom
        {
            //============= Find room with roomName from Client =========
            var isFoundRoom = false;
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    isFoundRoom = true;
                    break;
                }
            }
            //===========================================================

            if(isFoundRoom == true)// Found room
            {
                //Can't create room because roomName is exist.
                //========== Send callback message to Client ============

                //ws.send("CreateRoomFail"); 

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("client create room fail.");
            }
            else
            {
                //============ Create room and Add to roomList ==========
                var newRoom = {
                    roomName: toJsonObj.data,
                    wsList: []
                }

                newRoom.wsList.push(ws);

                roomList.push(newRoom);
                //=======================================================

                //========== Send callback message to Client ============

                //ws.send("CreateRoomSuccess");

                //I need to send roomName into client too. I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:toJsonObj.data
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
                console.log("client create room success.");
            }

            //console.log("client request CreateRoom ["+toJsonObj.data+"]");
            
        }
        else if(toJsonObj.eventName == "JoinRoom")//JoinRoom
        {
            //============= Home work ================
            // Implementation JoinRoom event when have request from client.
            
            //================= Hint =================
            //roomList[i].wsList.push(ws);
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    roomList[i].wsList.push(ws);

                    var callbackMsg = {
                        eventName:"JoinRoom",
                        data:toJsonObj.data
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    
                    console.log("client join room success.")
                    break;
                }
                if(i == roomList.length-1)
                {
                    var callbackMsg = {
                        eventName:"JoinRoom",
                        data:"fail"
                    }

                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);

                    console.log("client join room fail")
                    break;
                }
            }

            //============= Find room with roomName from Client =========

            console.log("client request JoinRoom");
            //========================================
        }
        else if(toJsonObj.eventName == "LeaveRoom")//LeaveRoom
        {
            //============ Find client in room for remove client out of room ================
            var isLeaveSuccess = false;//Set false to default.
            for(var i = 0; i < roomList.length; i++)//Loop in roomList
            {
                for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                {
                    if(ws == roomList[i].wsList[j])//If founded client.
                    {
                        roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                        if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                        {
                            roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                        }
                        isLeaveSuccess = true;
                        break;
                    }
                }
            }
            //===============================================================================

            if(isLeaveSuccess)
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomSuccess");

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"success"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room success");
            }
            else
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomFail");

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room fail");
            }
        }
        else if(toJsonObj.eventName == "Login"){
            var sqlSelect = "SELECT * FROM USERDATA WHERE UserID = '"+userID+"' AND Password= '"+password+"'"; //Login
            db.all(sqlSelect,(err, rows)=>
            {
                if(err)
                {
                console.log(err);
                }
                else
                {
                if(rows.length > 0)
                {
                    console.log("==========[1]========")
                    console.log(rows);
                    console.log("==========[1]========")
                    var callbackMsg = 
                    {
                        eventName: "Login",
                        data:rows[0].Name
                    }

                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    console.log("[2]" +toJsonStr);
                }
                else
                {
                    var callbackMsg = 
                    {
                        eventName: "Login",
                        data:"Fail"
                    }

                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    console.log("[3]" +toJsonStr);
                }
            
            }
        })
        }
        else if(toJsonObj.eventName == "Register"){
            var sqlInsert = `INSERT INTO UserData (UserID, Password, Name) VALUES ('${userID}', '${password}', '${name}')`;
            db.all(sqlInsert, (err, rows)=>{
                if(err){
                    console.log(err);

                    var callbackMsg = {
                        eventName:"Register",
                        data:"Fail"
                    }
                    //===============================================
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    //=======================================================
                    console.log("[0]" +toJsonStr);
                }
                else{
                    var callbackMsg = {
                        eventName:"Register",
                        data:"success"
                    }
                    //===============================================
                    var toJsonObj = JSON.stringify(callbackMsg);
                    ws.send(toJsonObj);
                    //=======================================================
                    console.log("[1]" +toJsonStr);
                }
            })
        }   
    });
    });


    //wsList.push(ws);
    
    /*ws.on("message", (data)=>{
        console.log("send from client :"+ data);
        Boardcast(data);
    });
    */
    wss.on("close", ()=>{
        console.log("client disconnected.");

        //============ Find client in room for remove client out of room ================
        for(var i = 0; i < roomList.length; i++)//Loop in roomList
        {
            for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
            {
                if(ws == roomList[i].wsList[j])//If founded client.
                {
                    roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                    if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                    {
                        roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                    }
                    break;
                }
            }
        }
        //===============================================================================
    });
});

function Boardcast(data)
{
    var selectRoomIndex = -1;

    for (var i = 0; i < roomList.length; i++)
    {
        for (var j = 0; j < roomList[i].wsList.length; j++)
        {
            if (ws == roomList[i].wsList[j])
            {
                selectRoomIndex = i;
                console.log(selectRoomIndex);
                break;
            }
        }
    }

    for (var i = 0; i < roomList[selectRoomIndex].wsList.length; i++)
    {
        roomList[selectRoomIndex].wsList[i].send(message);
    }
}

