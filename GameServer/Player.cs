using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Vector3 next_position;
        public Vector3 previus_position;
        public Quaternion rotation;
        int health;

        public struct PositionState
        {
            public bool[] inputs;
            public int packetId;
            public Quaternion rotation;
        }

        PositionState ps;
        private float moveSpeed = 0.1f;
        private bool[] inputs;
        bool isColliding = false;
        public MaguitoMap map;
        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            previus_position = _spawnPosition;
            rotation = Quaternion.Identity;
            map = new MaguitoMap();
            inputs = new bool[4];
            health = 3;

            ps.rotation = rotation;
            ps.inputs = inputs;
            ps.packetId = 0;
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if (ps.inputs[0])
            {
                _inputDirection.Y = 1;
            }
            if (ps.inputs[1])
            {
                _inputDirection.Y = -1;
            }
            if (ps.inputs[2])
            {
                _inputDirection.X = 1;
            }
            if (ps.inputs[3])
            {
                _inputDirection.X = -1;
            }
            //Check collison

                Move(ps.packetId,_inputDirection);

           
        }

        private void Move(int _packetId,Vector2 _inputDirection)
        {
            /* THIS IS SOME 3D SHIT
             * Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
             * Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));
             * Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;
             * position += _moveDirection * moveSpeed;*/

            Vector3 _moveDirection = new Vector3(_inputDirection.X, _inputDirection.Y, 0);
            next_position += _moveDirection * moveSpeed;
            foreach (Entity _en in map.entities)
            {
                if (_en.isColliding(new Vector2(next_position.X, next_position.Y+0.15f), 0.17f))
                {
                    isColliding = true;
                    break;
                }
                isColliding = false;
            }
            if (!isColliding)
            {
                position = next_position;
                previus_position = position;
                ServerSend.PlayerPosition(_packetId,this);
            }
            else
            {
                position = previus_position;
                next_position = previus_position; ;
                //ServerSend.PlayerPosition(this);
            }          
            ServerSend.PlayerRotation(this);
        }

        public void SetInput(int _packetId, bool[] _inputs, Quaternion _rotation)
        {
            ps = new PositionState
            {
                inputs = _inputs,
                packetId = _packetId,
                rotation = _rotation
            };

        
            /*inputs = _inputs;
            rotation = _rotation;
            packetId = _packetId;*/
        }

        public void Cast(Vector3 direction)
        {
            Console.WriteLine("Casting the fireball");
            Vector3 impact = direction-position;
            ServerSend.FireballImpact(this,impact);
        }
    }
}
