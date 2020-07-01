using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer
{
    public class MaguitoMap
    {
        public List<Entity> entities;

        public MaguitoMap()
        {
            entities = new List<Entity>(); 
            entities.Add(new Entity(0.76f, 0.72f, 0.16f));
            entities.Add(new Entity(-2.05f, 0.97f, 0.8f));
        }
    }

    public class Entity
    {
        Vector2 entityPosition;
        float r;

        public Entity(float _x, float _y, float _r)
        {
            entityPosition.X = _x;
            entityPosition.Y = _y;
            r = _r;
        }

        public Entity(Vector2 _entityPosition, float _r)
        {
            entityPosition = _entityPosition;
            r = _r;
        }

        public bool isColliding(Vector2 _position, float _radius)
        {
            float distance = Vector2.Distance(_position, entityPosition);
            Console.WriteLine($" distancia {distance}");
            Console.WriteLine($" distancia maxima centros{r + _radius}");
            if ( distance < r + _radius)
            {
                return true;
            }
            return false;
        }
    }
}
