using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Animazioni;
namespace Quantum_Game
{
    /// I colori del gioco... probabilmente inutile e da rimuovere, dato che esiste lo struct System.Color
	public enum e_color: byte
    {
		incolore,
		Blu,
		Rosso,
		Giallo,
		Verde
		
	}
	
    /// Tipi di nave
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

        public Nave(Giocatore proprietario)
        {
            _proprietario = proprietario;
            _ingioco = _mossa = _special = _riconfigurata = false;
            _tipo = e_nave.Rottame;
        }


        public int Pwr { get { return (int)_tipo; } }
        public e_nave Tipo { get { return this._tipo; } } // il nome della nave, nel caso dobbiamo scriverlo
        public bool Viva { get { return (_tipo > 0); } } // _tipo == 0 significa che la nave è un rottame attualmente
        public bool InGioco
        {
            get { return _ingioco; }

        }
        public bool Mossa { get { return _mossa; } }
        public bool SpecialUsata { get { return _special; } }
        public bool Riconfigurata { get { return _riconfigurata; } }
            // colori sia nel formato scemo che in quello System.Color
        public e_color Colore { get { return this._proprietario.Colore; } }
        public Microsoft.Xna.Framework.Color SpriteColor { get { return _proprietario.SpriteColor; } }

        // METODI PUBBLICI
        /// inizializzazione, da chiamare ogni inizio turno per tutte le navi di una flotta
        public void InizioTurno()
        {
            _riconfigurata = _mossa = _special = _muoveinDiagonale = false;
        }

        /// riconfigurazione della nave (o primo roll)
        public void Riconfigura(bool specialScout = false)
        {
            int risultato = 0;
            var TipoDiNaveIniziale = this._tipo;
            do
            {
                risultato = util.Dadi(1);
            } while (risultato == (int)TipoDiNaveIniziale);
            this._tipo = (e_nave)risultato;
            if (!specialScout) _riconfigurata = true;

            // Mi assicuro che, se uso la special della 5 e poi riconfiguro, la nave che ottengo non si possa muovere in diagonale
            _muoveinDiagonale = false;      
        }
        /// Metodo per piazzare per la prima volta una nave
        /// E' anche l'unico modo con cui una nave può essere dichiarata IN GIOCO
        public void Piazza(Casella CasellaTarget)
        {
            CasellaTarget.Occupante = this;
            CasellaOccupata = CasellaTarget;
            _ingioco = true;
            Piazzata?.Invoke(this, EventArgs.Empty);
        }
        /// Muove la nave da una casella a un'altra.
        public void Muovi(Casella CasellaPartenza, Casella CasellaTarget)
        {
            CasellaPartenza.Occupante = null;
            CasellaTarget.Occupante = this;
            CasellaOccupata = CasellaTarget;
            this._mossa = true;
        }

        /// <summary>
        /// Per rimuovere temporaneamente una nave dal gioco.
        /// La nave resta viva!!!
        /// </summary>
        public void RimuoviDalGioco()
        {
            CasellaOccupata.Occupante = null;
            this._ingioco = false;
        }

        /// Metodo per attaccare una nave target
        /// <param name="target">riferimento all'istanza di Nave da attaccare</param>
        /// <returns>Restituisce True se l'attacco è andato a buon fine</returns>
        bool Attacco(Nave target)
        {
            if (Pwr + util.Dadi(1) <= target.Pwr + util.Dadi(1))
            {
                target.Distruggi();
                this._proprietario.Dominio++;
                target._proprietario.Dominio--;
                return true;
            }
            return false;
        }
        public bool Attacco (Casella casella, bool consumaMovimento)
        {
            bool esito = Attacco(casella.Occupante);
            if (esito) casella.Occupante = null;
            if (consumaMovimento) this._mossa = true;
            return esito;
        }
        ///Usa la special. niente di sensazionale
        public void UsaSpecial()
        {
            this._special = true;
            if (_tipo == e_nave.Interceptor) _muoveinDiagonale = true;
        }
        /// La nave viene distrutta
		public void Distruggi() { _ingioco = false; Riconfigura(true); }

        //True se la nave è del colore del giocatore argomento
        public bool Alleato(Giocatore player) { return (_proprietario.Colore == player.Colore); }
        ///True se la nave può muoversi in diagonale
        public bool MuoveInDiagonale { get { return _muoveinDiagonale; } }

        public void Update ()
        {
            // spisellamento/Ondulazione
            _fase += INCREMENTO;
            if (_fase > 2.0) _fase = 2 - _fase;
            var seno = (float)(Math.Sin(_fase * Math.PI));
            offset = new Vector2(seno * 3.5f, 0);
        }

        /// <summary>
        /// Classico metodo Draw, con valori provvisori in attesa del tileset definitivo
        /// </summary>
        public void Draw (SpriteBatch spriteBatch, Texture2D texture, Vector2 coordinate, Vector2 scala )
        {
            if (_ingioco)
            {
                spriteBatch.Draw(
                    texture,
                    posizione + ( offset ) + scala * 50, // posizione + offset per spisellamenti; "scala*50" serve a centrare la nave nella casella
                    sourceRectangle: new Rectangle(300, 0, 100, 100),
                    scale: scala,
                    color: SpriteColor,
                    rotation: rotazione,
                    origin: scala * 100
                    );
            }
        }
        /// <summary>
        /// Metodo pubblico per aggiornare la posizione della nave sul tabellone
        /// </summary>
        public void Anima (Vector2 posizione, float rotazione)
        {
            this.posizione = posizione;
            this.rotazione = rotazione;
        }

        public event EventHandler Piazzata;

        // CAMPI 
        private Giocatore _proprietario;
        private bool _mossa, _special, _ingioco, _riconfigurata, _muoveinDiagonale;
		private e_nave _tipo;
        /// <summary>
        /// Usato solo dal ManagerNavi, per avere il riferimento di dove disegnare questa nave
        /// </summary>
        public Casella CasellaOccupata { get; private set; }
        /// <summary>
        /// Posizione sul tabellone della nave
        /// </summary>
        private Vector2 posizione;
        /// <summary>
        /// Rotazione del muso della nave
        /// </summary>
        private float rotazione = 0f;
        /// <summary>
        /// Per gli effetti di spisellamento/ondulazione
        /// </summary>
        private Vector2 offset = Vector2.Zero;
        /// <summary>
        /// fase dello spisellamento
        /// </summary>
        private float _fase = 0;
        /// <summary>
        /// incremento della fase ad ogni frame
        /// </summary>
        const float INCREMENTO = 0.007f;


    }
	
	
	
	
		
}
	

