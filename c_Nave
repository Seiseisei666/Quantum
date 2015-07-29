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
		private bool _ingioco, _mossa;
		private e_color _color;
		private e_nave _tipo;
		
		// i tiri di dado avvengono tutti a proposito dei metodi delle navi, per cui incorporo nella classe nave il RNG:
		private static Random _dado;
		private static int Dadi (int num = 1) {
			int res = 0;
			for (int i = 0; i < num; i++) {
				res += _dado.Next(1,6);
			}
			return res;
		}
		
		//costruttore
		public Nave (e_color c)
		{
			_ingioco = _mossa = false;
			_color = c;
			_tipo = e_nave.Rottame;
			_dado = new Random();
		}
		
		//inizializzazione, da chiamare ogni inizio turno per tutte le navi di una flotta
		public void init (){
			this._mossa = false;
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
			int res = 0;
			var prov = this._tipo;
			
			do {
				res = Dadi(1);
			} while (res == (int)prov);
			this._tipo = (e_nave) res;
			
		
		}
		
		public void Movim () {
			//fare cose varie per muovere la nave...
			this._mossa = true;
		}
		
		public bool Attacco (Nave target){
			this._mossa= true;
			if (this.Pwr + Dadi(1) <= target.Pwr + Dadi(1)) {
				target.Distr();
				return true;
			}
			return false;
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
		
	}
	
	/* Test per vedere se funzia
	 * 
	 * 
	 * 
	*/
	
	
		
	}
