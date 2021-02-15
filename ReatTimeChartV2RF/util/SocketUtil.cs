using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RealtimeChart.util
{
    public class SocketUtil
    {
        private static object _lockSend = new object();
        private static readonly Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);   //Socket 句柄

        private static readonly Socket clientSocketFace = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream, ProtocolType.Tcp);   //Socket 句柄
        ///<summary
        ///连接服务器，用于将手势坐标数据上传进行分析处理
        ///</summary>
        public static Socket connectToCNNServer()
        {
            try
            {
                if (clientSocket == null || !clientSocket.Connected)
                {
                    //if (clientSocket != null)
                    //{
                    //    clientSocket.Close();
                    //    clientSocket.Dispose();
                    //}
                    IPAddress ip = IPAddress.Parse(ItemString.host);
                    IPEndPoint ipe = new IPEndPoint(ip, ItemString.port);
                    clientSocket.SendTimeout = 2000;
                    clientSocket.ReceiveTimeout = 2000;
                    clientSocket.SendBufferSize = 1024;
                    clientSocket.ReceiveBufferSize = 1024;
                    clientSocket.Connect(ipe);
                }
                return clientSocket;
            }
            catch (Exception)
            {
                Console.WriteLine("connectToCNNServer error，服务器没有开启");
            }
            return null;
        }

        //public static Socket connectToFaceServer()
        //{
        //    try
        //    {
        //        if (clientSocketFace == null || !clientSocketFace.Connected)
        //        {
        //            //if (clientSocketFace != null)
        //            //{
        //            //    clientSocketFace.Close();
        //            //    clientSocketFace.Dispose();
        //            //}
        //            IPAddress ip = IPAddress.Parse(ItemString.host);
        //            IPEndPoint ipe = new IPEndPoint(ip, ItemString.portFace);
        //            clientSocketFace.SendTimeout = 2000;
        //            clientSocketFace.ReceiveTimeout = 2000;
        //            clientSocketFace.SendBufferSize = 1024;
        //            clientSocketFace.ReceiveBufferSize = 1024;
        //            clientSocketFace.Connect(ipe);
        //        }
        //        return clientSocketFace;
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("connectToFaceServer error，服务器没有开启");
        //    }
        //    return null;
        //}

        #region Send
        /// <summary>
        /// Send
        /// </summary>
        public static string SendReceive(string data)
        {
            lock (_lockSend)
            {
                //Encoding.ASCII.GetBytes(gestureDatas[gestureId].Id+";");
                System.Diagnostics.Debug.WriteLine("Send data=" + data);
                byte[] lenArr = Encoding.UTF8.GetBytes(data);
                int sendTotal = 0;
                while (sendTotal < lenArr.Length)
                {
                    int sendOnce = clientSocketFace.Send(lenArr, sendTotal, lenArr.Length - sendTotal, SocketFlags.None);
                    sendTotal += sendOnce;
                    Thread.Sleep(1);
                }
                System.Diagnostics.Debug.WriteLine("send data ok, data=" + Encoding.UTF8.GetString(lenArr));

                try
                {        // 发送方 发送字符串，文件名前缀
                    int block = 1024;
                    byte[] buffer = new byte[block];
                    int receiveCount = clientSocketFace.Receive(buffer, 0, block, SocketFlags.None);
                    if (receiveCount == 0)
                    {
                        return null;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("recieve data ok, data=" + buffer.ToString());
                        System.Diagnostics.Debug.WriteLine("recieve data ok, stringdata=" + Encoding.UTF8.GetString(buffer));
                        return Encoding.UTF8.GetString(buffer);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("接收数据出错：" + ex.Message + "\r\n" + ex.StackTrace);
                    return null;
                }

            }

        }
        #endregion



        #region 断开服务器
        /// <summary>
        /// 断开服务器clientSocketFace
        /// </summary>
        public static void DisconnectServer(Socket socket)
        {
            try
            {
                if (socket != null)
                {
                    if (socket.Connected)
                        socket.Disconnect(false);
                    socket.Close();
                    socket.Dispose();
                }
                System.Diagnostics.Debug.WriteLine("已断开服务器");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("断开服务器失败：" + ex.Message);
            }
        }
        #endregion
    }
}
