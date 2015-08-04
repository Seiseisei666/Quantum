using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public enum e_sett
    {
        vuoto,
        Pianeta7 = 7,
        Pianeta8,
        Pianeta9,
        Pianeta10
    }
    /// <summary>
    /// le 8 direzioni relative rispetto ad una casella
    /// </summary>
    public enum e_posizione
    {
        AltoSx,
        Alto,
        AltoDx,
        Sx,
        pianeta = -1,
        Dx = 5,
        BassoSx,
        Basso,
        BassoDx
    }

    public abstract class Zona
    {
        protected e_sett _tipo;
        public e_sett Tipo { get { return this._tipo; } }

    }

    /// <summary>
    /// Settore vuoto
    /// </summary>
    public class Vuoto: Zona
    {
        public Vuoto ()
        { _tipo = e_sett.vuoto; }
    }

    /// <summary>
    /// La casellona 3x3 del settore
    /// </summary>
    public class Settore: Zona
    {
        private e_color[] _colonizzazioni;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="tipo">Il tipo di settore (vuoto, pianeta da 7...)</param>
        public Settore(e_sett tipo)
        {
            _tipo = tipo;
            _colonizzazioni = new e_color[4];
            for (int i=0; i < _colonizzazioni.Length-1; i++) {
                _colonizzazioni[i] = e_color.incolore;
            }
        }



        /// <summary>
        /// controlla se ci sono spazi vuoti e se nessuna mentina è del giocatore;
        /// se entrambe le condizioni sono soddisfatte restituisce True (il pianeta può essere colonizzato dal giocatore)
        /// </summary>
        /// <param name="colore">Colore del giocatore su cui si fa il check</param>
        public bool Colonizzabile (e_color colore)
        {
            return _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
        }

        /// <summary>
        /// Colonizzazione del pianeta al centro di questo settore
        /// </summary>
        /// <param name="plyr">Riferimetno al giocatore colonizzatore</param>
        /// <returns>Se tutto va bene restituisce True</returns>
        public bool Colonizza (Giocatore plyr)
        {
            if (plyr.PuòAgire(true) && this.Colonizzabile (plyr.Colore)) {
                // Da togliere il check sulla condizione Colonizzabile? (lo fa il programma prima?)
                for (int i = 0; i < _colonizzazioni.Length - 1; i++)
                {
                    if (_colonizzazioni[i] == 0) { 
                        _colonizzazioni[i] = plyr.Colore;
                        Giocatore.Azione(true);                 // Toglie 2 azioni al giocatore
                        return true;
                    }
                }

            }
            return false;
        }


        public e_color [] Colonizzazioni
        {
            get { return _colonizzazioni; }
        }

        public bool Orbita (e_posizione p)
        {
            int pos = (int)p;
            if (pos == 1 || pos == 3 || pos == 5 || pos == 7) return true;
            return false;
        }

        public bool Orbita(int pos)
        {
            if (pos == 1 || pos == 3 || pos == 5 || pos == 7) return true;
            return false;
        }



    }
}
