﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;


namespace Network_Tester
{
    class ButtonFunctions
    {
        //Constructor
        public ButtonFunctions()
        {
            
        }

        //Method that generates a hash using SHA256
        public string GenHash(string cde, string Password)
        {

            //Place body of serializing code
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.GenerateKey();
            SymmetricKey.GenerateIV();

            // Encrypt the string to an array of bytes.
            byte[] encrypted = EncryptStringToBytes(cde, SymmetricKey.Key, SymmetricKey.IV);

            //Return the encrypted data
            return Encoding.Default.GetString(encrypted);
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            RijndaelManaged rijAlg = new RijndaelManaged();
            
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for encryption.
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            StreamWriter swEncrypt = new StreamWriter(csEncrypt);

            //Write all data to the stream.
            swEncrypt.Write(plainText);
            encrypted = msEncrypt.ToArray();
                    
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        //Method that checks IP address on local machine
        public string[] IpCheck()
        {
            string[] response = new string[2];
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    response[0] = ip.ToString();
                }
            }
            response[1] = host.HostName;
            return response;
        }

        public List<int> ServerConnect()
        {

            List<int> listOfPorts = new List<int>();
            int port = 10;
            TcpListener server = null;

            //Assign localhost ip address to ipa
            IPAddress ipa = Dns.GetHostAddresses("localhost")[1];
            try
            {

                server = new TcpListener(ipa, 10);
                server.Start();
                //viewModel.Textblock("Connection to server started...");

                TcpClient client = new TcpClient("localhost", port);
                listOfPorts.Add(port);
                client.Close();
                server.Stop();

            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
            return listOfPorts;
        }

    }
}
