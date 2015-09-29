using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

/// <summary>
/// Serve per tenere traccia delle caselle esistenti.
/// Vengono organizzate in una lista. 
/// </summary>

namespace Quantum_Game
{


    /// <summary>
    /// Tutti i tipi di caselle ereditano da un modello astratto, la classe Tile,
    /// che contiene il campo pubblico Tipo e un altro paio di cosette overridabili
    /// </summary>
    public class Casella : Tile
    {
        // override di Tile
        public override bool Esistente { get { return true; } }
        public override bool EunaCasella { get { return true; } }
        public override bool PresenzaAlleata (Nave nave) { return (_occupante != null && _occupante.Colore == nave.Colore); }
        public override bool PresenzaAlleata(Giocatore giocatore) { return (_occupante != null && _occupante.Colore == giocatore.Colore); }

        //campi propri
        private Nave _occupante;
        public Nave Occupante { get { return _occupante; } set { _occupante = value; } }

        public bool Orbita { get { return _tipo == QuantumTile.orbita; } }

        //costruttore
        public Casella(QuantumTile tipo)
        {
            _tipo = tipo;
            _occupante = null;
        }

        /// <summary>
        /// Disegna la casella
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 coordinate, Vector2 scala)
        {
            Rectangle source;
            if (Orbita)
                source = new Rectangle(100, 0, 100, 100);
            else
                source = new Rectangle(0, 0, 100, 100);
        
            spriteBatch.Draw(texture, coordinate, sourceRectangle: source, scale: scala);
        }
    }
}

