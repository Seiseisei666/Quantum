using Quantum_Game;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Quantum_Game {


public class Giocatore {
        // costanti eterne immutabili
		const byte NUM_AZIONI = 3;
		const byte PUNTI_x_VINCERE = 10;
		
        // contatori per il numero dei giocatori e le azioni disponibili per turno
		private static byte _count;
		private static byte _azioni;
        public static byte AzioniDisponibili { get { return _azioni; } }

        // variabili vere del giocatore
		private int _ricerca, _dominio, _punti;
        private List<Nave> _flotta;
        public int NumeroNavi { get { return _flotta.Count; } }

        // colore giocatore
        private e_color _colore;
		public e_color Colore { get {return this._colore;} }
        public Color SpriteColor { get { return GameSystem.QuantumColor[_colore]; } }

		//costruttore del giocatore
		public Giocatore () {
			this._colore = (e_color) (++_count); //assegna il colore
			_ricerca = _dominio = _punti = 0;
			_flotta = new List<Nave> ();
        }
		
		public void GlobalInit () {
            // inizializzazione prima che inizi il gioco 
            InizializzaFlotta();

		}

        /// <summary>
        /// metodo generale per mettere in gioco e riconfigurare nuove navi
        /// </summary>
        public void NuoveNavi(int numeroNuoveNavi = 1)
        {
            for (int i = 0; i < numeroNuoveNavi; i++)
            {
                Nave n = new Nave(this);
                _flotta.Add(n);
                n.Riconfig();
             }
		}
        /// <summary>
        /// restituisce una ad una le navi che sono state rollate ma aspettano di essere messe in gioco
        /// se non ce ne sono restituisce NULL
        /// </summary>
        public Nave NaveDaPiazzare { get { return _flotta.Find(x => x.InGioco == false); } }

        private void InizializzaFlotta (int NUMERO_NAVI_INIZIALI = 3)
        {
            if (_flotta.Count > 0) return;
            NuoveNavi(NUMERO_NAVI_INIZIALI);
        }

        /// <summary>
        /// inizializzazione, da chiamare prima di ogni turno
        /// </summary>
        public void Init () {
			
			foreach (var n in _flotta) {n.init();}
			_azioni = NUM_AZIONI;
		}

        public void Cleanup()
        {
            if (_dominio >= 5)
            {
                _dominio -= 5; //reset dominio
                // Piazzare la mentina
            }
            if (_punti >= PUNTI_x_VINCERE)
            {
                //Fine Gioco
            }
        }

        /// <summary>
        /// Restituisce True se il giocatore ha abbastanza punti azione
        /// </summary>
        /// <param name="colonizzazione">Se l'azione è una colonizzazione passare questo parametro True</param>
        static public bool PuòAgire(bool colonizzazione = false)
        {
            if (colonizzazione) return _azioni >= 2;
            return _azioni > 0;

        }

        /// <summary>
        /// diminuisce il contatore delle azioni di uno, o di due se è un'azione colonizza
        /// </summary>
        /// <param name="colonizzazione">Se l'azione da fare è una colonizzazione passare True come parametro</param>
        public static void Azione (bool colonizzazione = false)
        {
            _azioni--;
            if (colonizzazione) _azioni--;
            if (_azioni <= 0)
            {
                //finisce il turno
            }
        }

        


    }
		
		
}
	
