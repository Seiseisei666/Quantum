using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Quantum_Game
{
    /// <summary>
    /// Classe dei messaggi mouse inviati con gli eventi del mouse
    /// racchiude tutte le informazioni su posizione e bottoni premuti
    /// ricalcata su una classe per le form microsoft più massiccia di così, con cose tipo
    /// la scroll wheel che a noi non servono
    /// </summary>
    public enum TipoEventoMouse
    {
        ClkSin,
        ClkDx,
        Over
    }


    public class MouseEvntArgs : System.EventArgs
    {
        private Point _posizione;
        private bool _sinistroPremuto;
        private bool _destroPremuto;

        public MouseEvntArgs (int x, int y, bool sin, bool dx)
        {
            _posizione.X = x; _posizione.Y = y;
            _sinistroPremuto = sin; _destroPremuto = dx;
        }

        public Point Posizione { get { return _posizione; } }
        public bool TastoSinistro { get { return _sinistroPremuto; } }
        public bool TastoDestro { get { return _destroPremuto; } }
    }
}
