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
            if (_count >= 4)
            {
                throw new System.Exception("Il numero massimo di giocatori è 4!!!");
            } 

			this._colore = (e_color) (++_count); //assegna il colore
			_ricerca = _dominio = _punti = 0;
			Flotta = new List<Nave> ();
			
		}
		
		public void GlobalInit () {
		// inizializzazione prima che inizi il gioco 


		}
		
		//metodo generale per piazzare una NUOVA nave
		public void PiazzaNuovaNave() {
			Nave n = new Nave(this);
				Flotta.Add (n);
				Flotta[Flotta.Count-1].Gioca();
				Flotta[Flotta.Count-1].Riconfig();
            //Qua aspettiamo l'interazione del giocatore: 
            //richiediamo il click per posizionare la nave
				
			}
		
		
		//inizializzazione prima di ogni turno
		public void init () {
			
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
     
            // diminuisce il contatore delle azioni di uno, o di due se è un'azione colonizza
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
	
