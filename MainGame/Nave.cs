using System;
namespace Quantum_Game
{
	/// <summary>
    /// I colori del gioco... probabilmente inutile e da rimuovere, dato che esiste lo struct System.Color
    /// </summary>
	public enum e_color: byte {
		incolore,
		Blu,
		Rosso,
		Giallo,
		Verde
		
	}
	
    /// <summary>
    /// Tipi di nave. Rottame == 0 == nave nel cimitero (o non ancora giocata)
    /// </summary>
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
	public class Nave
	{
        private bool _ingioco, _mossa, _special;
		private e_nave _tipo;
		private Giocatore _proprietario;
		
		/// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="prop">riferimento al giocatore proprietario della pedina</param>
		public Nave (Giocatore prop)
		{
			this._proprietario = prop;
			_ingioco = _mossa = _special = false;
			_tipo = e_nave.Rottame;
		}

        /// <summary>
        /// inizializzazione, da chiamare ogni inizio turno per tutte le navi di una flotta
        /// </summary>
        public void init (){
			this._mossa = false;
            this._special = false;
		}
		
		/// <summary>
        /// Restituisce il valore della nave (1...6)
        /// </summary>
		public int Pwr {
			get {return (int)this._tipo;}
		}
        /// <summary>
        /// Restituisce il tipo della nave (enum e_nave: Rottame, Battlestation...)
        /// </summary>
		public e_nave Tipo {
			get {return this._tipo;}
		}

        /// <summary>
        /// riconfigurazione della nave (o primo roll)
        /// </summary>
        public void Riconfig () {
			int risultato = 0;
			var TipoDiNaveIniziale = this._tipo;
			do {
				risultato = util.Dadi(1);
			} while (risultato == (int)TipoDiNaveIniziale);
			this._tipo = (e_nave) risultato;
		}
		
		public void Movim (Casella CasellaTarget) {
            CasellaTarget.Occupante = this;
			this._mossa = true;
            Giocatore.Azione();
		}

		/// <summary>
        /// Metodo per attaccare una nave target
        /// </summary>
        /// <param name="target">riferimento all'istanza di Nave da attaccare</param>
        /// <returns>Restituisce True se l'attacco è andato a buon fine</returns>
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
		
		public bool Viva {  get {return (this._tipo > 0);}}

        /// <summary>
        /// La nave viene distrutta
        /// </summary>
		public void Distr () {  this._tipo = e_nave.Rottame;}

        /// <summary>
        /// Restituisce True se la nave ha già mosso in questo turno
        /// </summary>
		public bool Mossa { get {return this._mossa;}  }
        /// <summary>
        /// Restituisce True se la nave è in gioco al momento
        /// </summary>
		public bool InGioco {   get {return this._ingioco;} }
		
        /// <summary>
        /// Mette in gioco la nave
        /// </summary>
		public void Gioca (){
			this._ingioco = true;
		}
		/// <summary>
        /// restituisce il colore del giocatore che possiede la nave
        /// </summary>
		public e_color Colore {get {return this._proprietario.Colore;}}
        public Microsoft.Xna.Framework.Color SpriteColor
        {
            get
            {
                return _proprietario.SpriteColor;
            }
        }

    }
	
	
	
	
		
	}
	

