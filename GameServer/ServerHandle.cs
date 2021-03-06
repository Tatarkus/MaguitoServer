﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            int _packetId = _packet.ReadInt();
            bool[] _inputs = new bool[_packet.ReadInt()];
            //Server.clients[_fromClient].lastAck = _packetId;
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            Quaternion _rotation = _packet.ReadQuaternion();
            try
            {
                //TODO: fix crash
                if (Server.clients[_fromClient].player != null)
                Server.clients[_fromClient].player.SetInput(_packetId,_inputs, _rotation);
            }
            catch (Exception)
            {


            }
            
        }

        public static void PlayerFireball(int _fromClient, Packet _packet)
        {
            Vector3 _target;
            int _caster;
            
            _caster = _packet.ReadInt();
            _target = _packet.ReadVector3();
            Console.WriteLine($"Player {_caster} sent a fireball to: {_target}");
            Server.clients[_caster].player.Cast(_target);
        }

    }
}
