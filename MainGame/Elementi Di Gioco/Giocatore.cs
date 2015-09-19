using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace Quantum_Game {

//TODO rifare la gestione dei colori!
public class Giocatore
    {

        // COSTRUTTORI
        //Costruttore di default
        public Giocatore()
        {
            //assegna il colore
            this._colore = (e_color)(++_count); 
        }
        // Costruttore per fare giocatori di colori particolari
        public Giocatore (e_color colore)   
        {
            _colore = colore;
        }

        // PROPRIETA' PUBBLICHE
        public int AzioniDisponibili { get { return _azioni; } }

        // Numero di navi in gioco del giocatore
        public int NumeroNaviInGioco
        { get
            {
                var vive = _flotta.FindAll(nave => nave.Viva);
                return vive.Count;
            }
        }
        // restituisce, se ce n'è, una nave giocata ma non ancora posizionata sulla plancia
        public Nave NaveDaPiazzare { get { return _flotta.Find(x => x.InGioco == false); } } 

        /// Lista delle navi del giocator nel cimitero
        public List<Nave> Rottami { get { return _flotta.FindAll(n => !n.InGioco); } }

        public e_color Colore { get { return this._colore; } }
        public Color SpriteColor { get { return GestoreDiGiocatori.QuantumColor[_colore]; } }

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
        /* Restituisce un ID (nome e/o colore) del giocatore */
        public override String ToString()
        {
            return this.Colore.ToString();
        }
        // inizializzazione globale
        public void GlobalInit() 
        {
            inizializzaFlotta();
        }

        /// metodo generale per mettere in gioco e riconfigurare nuove navi
        public void NuoveNavi(int numeroNuoveNavi = 1)
        {
            for (int i = 0; i < numeroNuoveNavi; i++)
            {
                Nave n = new Nave(this);
                _flotta.Add(n);
                n.Riconfigura();
            }
        }

        /// Inizio Turno
        public void Init()
        {
            foreach (var n in _flotta)
                n.InizioTurno(); 
            _azioni = NUM_AZIONI;
        }

        /// diminuisce il contatore delle azioni di uno, o di due se è un'azione colonizza
        /// <param name="colonizzazione">Se l'azione da fare è una colonizzazione passare True come parametro</param>
        public void Azione(bool colonizzazione = false)
        {
            _azioni--;
            if (colonizzazione)
                _azioni--;
        }

        public void Ricerca ()
        {
            _ricerca++;
             System.Diagnostics.Debug.WriteLine(_ricerca);
        }

        // Fine turno
        public void Cleanup()  
        {
            // TODO: probabilmente questo metodo non ha senso, lo lascio vuoto per sicurezza
            // Sarà il flusso di gioco a controllare le condizioni e a gestire, quando necessario, il cleanup di fine turno

        }
        // METODI PRIVATI
        private void inizializzaFlotta(int NUMERO_NAVI_INIZIALI = 3)
        {
            if (_flotta.Count > 0) return;
            NuoveNavi(NUMERO_NAVI_INIZIALI);
        }

        // CAMPI PRIVATI
		private int _ricerca, _dominio, _punti=0;
        private List<Nave> _flotta = new List<Nave>();
        private e_color _colore;

        // contatori statici
        static byte _count; // num giocatori
        static int _azioni;// azioni disponibili x turno

        // costanti eterne immutabili
        const int NUM_AZIONI = 3;
        const byte PUNTI_x_VINCERE = 10;
    }
		
		
}
	
