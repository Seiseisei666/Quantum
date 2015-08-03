using System;
namespace Quantum_Game
{
	
	public enum e_color: byte {
		incolore,
		Blu,
		Rosso,
		Giallo,
		Verde
		
	}
	
	public enum e_nave: byte {
		Rottame,
		Battlestation,
		Flagship,
		Destroyer,
		Frigate,
		Interceptor,
		Scout
	}
	
	
	/// <summary>
	/// Classe Nave - le pedine del gioco
	/// </summary>
	/// 

	public class Nave
	{
        private bool _ingioco, _mossa, _special;
	//	private e_color _color;
		private e_nave _tipo;
		private Giocatore _proprietario;
		
		
		
		//costruttore
		public Nave (Giocatore prop)
		{
			this._proprietario = prop;
			_ingioco = _mossa = false;
			_tipo = e_nave.Rottame;
			
		}
		
		//inizializzazione, da chiamare ogni inizio turno per tutte le navi di una flotta
		public void init (){
			this._mossa = false;
            this._special = false;
		}
		
		//metodo get per leggere il valore della nave
		public int Pwr {
			get {return (int)this._tipo;}
		}
		public e_nave Tipo {
			get {return this._tipo;}
		}
		
		//riconfigurazione delle navi (o primo roll se viene chiamata senza argomenti)
		public void Riconfig () {
			int risultato = 0;
			var prov = this._tipo;
			
			do {
				risultato = util.Dadi(1);
			} while (risultato == (int)prov);
			this._tipo = (e_nave) risultato;
			
		
		}
		
		public void Movim (Casella CasellaTarget) {
			//fare cose varie per muovere la nave...
			this._mossa = true;
            Giocatore.Azione();
           
		}
		
		public bool Attacco (Nave target){
			this._mossa= true;
            Giocatore.Azione();
            
			if (this.Pwr + util.Dadi(1) <= target.Pwr + util.Dadi(1)) {
				target.Distr();
				return true;
			}
			return false;
		}

        public void Special()
        {
            //usare la special
            this._special = true;
        }
		
		public bool Viva {
			get {return (this._tipo > 0);}
		}
		public void Distr () {
			this._tipo = e_nave.Rottame;
		}
		public bool Mossa {
			get {return this._mossa;}
		}
		public bool InGioco {
			get {return this._ingioco;}
		}
		
		public void Gioca (){
			this._ingioco = true;
		}
		
		public e_color Colore {get {return this._proprietario.Colore;}}
		
	}
	
	
	
	
		
	}
	

