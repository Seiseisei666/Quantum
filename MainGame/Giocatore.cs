﻿using Quantum_Game;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Quantum_Game {


public class Giocatore {
        
		const byte NUM_AZIONI = 3;
		const byte PUNTI_x_VINCERE = 10;
		
		private static byte _count;
		private static byte _azioni;
        public static byte AzioniDisponibili { get { return _azioni; } }
		private int _ricerca, _dominio, _punti;
		private e_color _colore;
		
		public e_color Colore {
			get {return this._colore;}
		}

        private List<Nave> _flotta;
        public int NumeroNavi { get { return _flotta.Count; } }

        public List<Nave> Flotta { get { return _flotta; } }
		
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

        private void InizializzaFlotta (int NUMERO_NAVI_INIZIALI = 3)
        {
            if (_flotta.Count > 0) return;
            NuoveNavi(NUMERO_NAVI_INIZIALI);
        }


        /// <summary>
        /// inizializzazione, da chiamare prima di ogni turno
        /// </summary>
        public void Init () {
			
			foreach (var n in Flotta) {n.init();}
			_azioni = NUM_AZIONI;
			
		}

        public void Cleanup()
        {
            if (_punti >= PUNTI_x_VINCERE)
            {

                //Fine Gioco

            }
        }

        /// <summary>
        /// Restituisce True se il giocatore ha abbastanza punti azione
        /// </summary>
        /// <param name="colonizzazione">Se l'azione è una colonizzazione passare questo parametro True</param>
        public bool PuòAgire(bool colonizzazione = false)
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

        public Color SpriteColor
        {
            get
            {
                return GameSystem.QuantumColor[_colore];
            }
        }


    }
		
		
}
	
