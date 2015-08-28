using Quantum_Game;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Quantum_Game {


public class Giocatore {

        // COSTRUTTORE
        public Giocatore()
        {
            this._colore = (e_color)(++_count); //assegna il colore
            _ricerca = _dominio = _punti = 0;
            _flotta = new List<Nave>();
        }
        public Giocatore (e_color colore)   // Overload del costruttore in caso volessimo fare giocatori di colori particolari
        {
            _colore = colore;
            _ricerca = _dominio = _punti = 0;
            _flotta = new List<Nave>();
        }

        // PROPRIETA' PUBBLICHE
        public int AzioniDisponibili { get { return _azioni; } }

        /// <summary>Numero di navi in gioco</summary>
        public int NumeroNaviInGioco
        { get
            {
                var vive = _flotta.FindAll(nave => nave.Viva);
                return vive.Count;
            }
        } 
        public Nave NaveDaPiazzare { get { return _flotta.Find(x => x.InGioco == false); } } // restituisce, se ce n'è, una nave giocata ma non ancora posizionata sulla plancia

        public e_color Colore { get { return this._colore; } }
        public Color SpriteColor { get { return GameSystem.QuantumColor[_colore]; } }

        public bool PuòAgire { get { return _azioni > 0; } }
        public bool PuòColonizzare { get { return _azioni > 1; } }

        public int Dominio
        {
            get
            { return _dominio; }
            set // accetta solo incrementi o decrementi unitari
            {   if (System.Math.Abs(value - _dominio) == 1)
                    _dominio = value;
                if (_dominio < 0) _dominio = 0;
                System.Diagnostics.Debug.WriteLine("Punti Dominio del giocatore {0}: {1}", this.Colore, _dominio);
            }
        }

        //METODI PUBBLICI 
        public void GlobalInit() // inizializzazione globale
        {
            inizializzaFlotta();
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
        /// Inizio Turno
        /// </summary>
        public void Init()
        {
            foreach (var n in _flotta)
                n.init(); 
            _azioni = NUM_AZIONI;
        }
        /// <summary>
        /// diminuisce il contatore delle azioni di uno, o di due se è un'azione colonizza
        /// </summary>
        /// <param name="colonizzazione">Se l'azione da fare è una colonizzazione passare True come parametro</param>
        public void Azione(bool colonizzazione = false)
        {
            _azioni--;
            if (colonizzazione)
                _azioni--;
        }
        public void Cleanup()   // Fine turno
        {
            if (_dominio >= 5)
            {
                _dominio -= 5; //reset dominio
                // Piazzare la mentina
            }
        }
        // METODI PRIVATI
        private void inizializzaFlotta(int NUMERO_NAVI_INIZIALI = 3)
        {
            if (_flotta.Count > 0) return;
            NuoveNavi(NUMERO_NAVI_INIZIALI);
        }

        // CAMPI PRIVATI
		private int _ricerca, _dominio, _punti;
        private List<Nave> _flotta;
        private e_color _colore;

        // contatori statici
        static byte _count; // num giocatori
        static int _azioni;// azioni disponibili x turno

        // costanti eterne immutabili
        const int NUM_AZIONI = 3;
        const byte PUNTI_x_VINCERE = 10;
    }
		
		
}
	
