using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestWriter : MonoBehaviour {

    // Writing data to a NetworkWriter and then
    // Converting this to a NetworkReader.
    void Start()
    {
        // The data you add to your writer must be prefixed with a message type.
        // This is in the form of a short.
        short myMsgType = 143;

        NetworkWriter writer = new NetworkWriter();

        // You start the message in your writer by passing in the message type.
        // This is a short meaning that it will take up 2 bytes at the start of
        // your message.
        writer.StartMessage(myMsgType);


        // You can now begin your message. In this case we will just use strings.
        //writer.Write(new Vector3(3.0f, 1.44f, 2.56f));
        writer.Write("Test data 2");
        //writer.Write("Test data 3");


        // Make sure to end your message with FinishMessage()
        writer.FinishMessage();


        // You can now access the data in your writer. ToArray() returns a copy
        // of the bytes that the writer is using and AsArray() returns the
        // internal array of bytes, not a copy.
        byte[] writerData = writer.ToArray();
        CreateNetworkReader(writerData);
    }

    void CreateNetworkReader(byte[] data)
    {
        // We will create the NetworkReader using the data from our previous
        // NetworkWriter.
        NetworkReader networkReader = new NetworkReader(data);


        // The first two bytes in the buffer represent the size
        // of the message. This is equal to the NetworkReader.Length
        // minus the size of the prefix.
        byte[] readerMsgSizeData = networkReader.ReadBytes(2);
        short readerMsgSize = (short)((readerMsgSizeData[1] << 8) + readerMsgSizeData[0]);
        Debug.Log(readerMsgSize);


        // The message type added in NetworkWriter.StartMessage
        // is to be read now. It is a short and so consists of
        // two bytes. It is the second two bytes on the buffer.
        byte[] readerMsgTypeData = networkReader.ReadBytes(2);
        short readerMsgType = (short)((readerMsgTypeData[1] << 8) + readerMsgTypeData[0]);
        Debug.Log(readerMsgType);

        // If all of your data is of the same type (in this case the
        // data on our buffer is comprised of only strings) you can
        // read all the data from the buffer using a loop like so.
        while (networkReader.Position < networkReader.Length)
        {
            Debug.Log(networkReader.ReadString());
        }

        Debug.Log("MsgType.Connect " + MsgType.Connect);
        Debug.Log("MsgType.Error " + MsgType.Error);
        Debug.Log("MsgType.NetworkInfo " + MsgType.NetworkInfo);
        Debug.Log("MsgType.ObjectSpawn " + MsgType.ObjectSpawn);
        Debug.Log("MsgType.ObjectSpawnScene " + MsgType.ObjectSpawnScene);
        Debug.Log("MsgType.SpawnFinished " + MsgType.SpawnFinished);
    }
}
