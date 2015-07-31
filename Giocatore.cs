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
		
		
		public Giocatore (){
			this._colore = (e_color)((_count++) % 4)+1;
			_azioni = 0;
			_ricerca = _dominio = _punti = 0;
			Flotta = new List<Nave> ();
			
		}
		
		public void GlobalInit () {
		// inizializzazione prima che inizi il gioco 
		}
		
		
		public void PiazzaNave() {
			Nave n = new Nave(this);
				Flotta.Add (n);
				Flotta[Flotta.Count-1].Gioca();
				Flotta[Flotta.Count-1].Riconfig();
				//Interazione del giocatore: richiediamo il click per posizionare la nave
				
			}
		
		
		//inizializzazione prima di ogni turno
		public void init () {
			
			foreach (var n in Flotta) {n.init();}
			_azioni = NUM_AZIONI;
			
		}
		
		public void cleanup() {
			if (_punti>=PUNTI_x_VINCERE) {
				
				//Fine Gioco
				
			}
			
			
		}
		
		
}
	
}