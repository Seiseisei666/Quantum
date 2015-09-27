using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    public class ManagerNavi: IDisposable
    {
        public ManagerNavi(Func<Tile,Vector2> tile2Posizione, Vector2 scala)
        {
            _scala = scala;
            tile2posizione = tile2Posizione;
            _navi = new Nave[0];
        }

        public void Aggiungi (Nave nave)
        {
            _navi = _navi.Concat(new Nave[] { nave }).ToArray();
        }

        public void Update()
        {
            foreach (Nave nave in _navi)
                nave.Update();
        }

        public void Draw (SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (Nave nave in _navi)
            {
                var pos = tile2posizione ( nave.Posizione);
                nave.Draw(spriteBatch, texture, pos, _scala);
            }

        }
        
        public void Dispose()
        {
            _navi = null;
            tile2posizione = null;
        }

        Func<Tile, Vector2> tile2posizione;
        Nave[] _navi;
        Vector2 _scala;

    }
}
