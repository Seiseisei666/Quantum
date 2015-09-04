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


	public class Nave
	{
        // COSTRUTTORE
        public Nave(Giocatore proprietario)
        {
            this._proprietario = proprietario;
            _ingioco = _mossa = _special = _riconfigurata = false;
            _tipo = e_nave.Rottame;
        }

        // PROPRIETA' PUBBLICHE
            // tutte le varie informazioni che abbiamo sulla nave
        public int Pwr { get { return (int)_tipo; } }
        public e_nave Tipo { get { return this._tipo; } } // il nome della nave, nel caso dobbiamo scriverlo
        public bool Viva { get { return (_tipo > 0); } } // _tipo == 0 significa che la nave è un rottame attualmente
        public bool InGioco { get { return _ingioco; } }
        public bool Mossa { get { return _mossa; } }
        public bool SpecialUsata { get { return _special; } }
        public bool Riconfigurata { get { return _riconfigurata; } }
            // colori sia nel formato scemo che in quello System.Color
        public e_color Colore { get { return this._proprietario.Colore; } }
        public Microsoft.Xna.Framework.Color SpriteColor { get { return _proprietario.SpriteColor; } }

        // METODI PUBBLICI
        /// <summary>
        /// inizializzazione, da chiamare ogni inizio turno per tutte le navi di una flotta
        /// </summary>
        public void init()
        {
            _riconfigurata = _mossa = _special = _muoveinDiagonale = false;
        }
        /// <summary>
        /// riconfigurazione della nave (o primo roll)
        /// </summary>
        public void Riconfig(bool specialScout = false)
        {
            int risultato = 0;
            var TipoDiNaveIniziale = this._tipo;
            do
            {
                risultato = util.Dadi(1);
            } while (risultato == (int)TipoDiNaveIniziale);
            this._tipo = (e_nave)risultato;
            if (!specialScout) _riconfigurata = true;
            _muoveinDiagonale = false;      // Mi assicuro che se uso la special della 5 e poi riconfiguro, la nave che ottengo non si possa muovere in diagonale
        }
        /// <summary>
        /// Metodo per piazzare per la prima volta una nave
        /// E' l'unico modo con cui una nave può essere dichiarata IN GIOCO
        /// </summary>
        public void Piazza(Casella CasellaTarget)
        {
            CasellaTarget.Occupante = this;
            _ingioco = true;
        }
        public void Muovi(Casella CasellaPartenza, Casella CasellaTarget)
        {
            CasellaPartenza.Occupante = null;
            CasellaTarget.Occupante = this;
            this._mossa = true;
        }
        /// <summary>
        /// Metodo per attaccare una nave target
        /// </summary>
        /// <param name="target">riferimento all'istanza di Nave da attaccare</param>
        /// <returns>Restituisce True se l'attacco è andato a buon fine</returns>
        public bool Attacco(Nave target)
        {
            this._mossa = true;
            if (this.Pwr + util.Dadi(1) <= target.Pwr + util.Dadi(1))
            {
                target.Distruggi();
                this._proprietario.Dominio++;
                target._proprietario.Dominio--;
                return true;
            }
            return false;
        }
        public bool Attacco (Casella casella)
        {
            bool risultato = Attacco(casella.Occupante);
            if (risultato)
                casella.Occupante = null;
            return risultato;
        }

        public void UsaSpecial()
        {
            //usare la special
            this._special = true;
            if (_tipo == e_nave.Interceptor) _muoveinDiagonale = true;
        }
        /// <summary>
        /// La nave viene distrutta
        /// </summary>
		public void Distruggi() { this._tipo = e_nave.Rottame; }

        public void Gioca() { _ingioco = true; }
            // per vedere se la nave è del colore di un giocatore
        public bool Alleato(Giocatore player) { return (_proprietario.Colore == player.Colore); }
        /// <summary>True se la nave può muoversi in diagonale</summary>
        public bool MuoveInDiagonale { get { return _muoveinDiagonale; } }
        // CAMPI 
        private Giocatore _proprietario;
        private bool _mossa, _special, _ingioco, _riconfigurata, _muoveinDiagonale;
		private e_nave _tipo;
        

    }
	
	
	
	
		
	}
	

