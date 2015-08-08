using Quantum_Game;
using System.Collections.Generic;

namespace Quantum_Game {

public class Giocatore {
		
		const byte NUM_AZIONI = 3;
		const byte PUNTI_x_VINCERE = 10;
		
		private static byte _count;
		private static byte _azioni;
		private int _ricerca, _dominio, _punti;
		private e_color _colore;
		
		public e_color Colore {
			get {return this._colore;}
		}
		
		public List<Nave> Flotta;
		
		//costruttore del giocatore
		public Giocatore (){
			this._colore = (e_color) (++_count); //assegna il colore
			_ricerca = _dominio = _punti = 0;
			Flotta = new List<Nave> ();
		}
		
		public void GlobalInit () {
		// inizializzazione prima che inizi il gioco 


		}

        /// <summary>
        /// metodo generale per mettere in gioco e riconfigurare una NUOVA nave
        /// </summary>
        public void PiazzaNuovaNave() {
			Nave n = new Nave(this);
				Flotta.Add (n);
				Flotta[Flotta.Count-1].Gioca();
				Flotta[Flotta.Count-1].Riconfig();
            //Qua aspettiamo l'interazione del giocatore: 
            //richiediamo il click per posizionare la nave
				
			}


        /// <summary>
        /// inizializzazione, da chiamare prima di ogni turno
        /// </summary>
        public void Init () {
			
			foreach (var n in Flotta) {n.init();}
			_azioni = NUM_AZIONI;
			
		}

        public void cleanup()
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
			
			
		}
		
		
}
	
