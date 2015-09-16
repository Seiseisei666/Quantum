using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    
    public enum FasiDiGioco
    {
        SceltaOpzioni = -1, SetupPartita, PartitaInCorso
    }

    /// Con questa classe gestiamo le fasi di gioco, i turni e i giocatori della partita
    /// Importanti la proprietà GiocatoreDiTurno (l'unico riferimento pubblico ai giocatori)
    /// e quella FasePartita.
    /// Invia un evento InizioPartita per informare tutto il programma che si comincia a giocare
    public class GameSystem
    {
        private static int _contaTurni = 0;
        private List<Giocatore> _giocatori = new List<Giocatore>();
        private FasiDiGioco _faseDiGioco = FasiDiGioco.SceltaOpzioni;
        private int _numGiocatori = 0;

        // soluzione non molto elegante. Cmq converte i nostri colori nel formato System.Color
        public static Dictionary<e_color, Color> QuantumColor = new Dictionary<e_color, Color>();

        
        public event EventHandler InizioPartita;

        public GameSystem()
        {
            //Definizione dei colori dei giocatore?
            QuantumColor.Add(e_color.Blu, Color.Blue);
            QuantumColor.Add(e_color.Rosso, Color.Red);
            QuantumColor.Add(e_color.Giallo, Color.Yellow);
            QuantumColor.Add(e_color.Verde, Color.Green);
        }

        //riferimento al giocatore di turno 
        public Giocatore GiocatoreDiTurno { get { return _giocatori[(_contaTurni % _giocatori.Count)]; } }
        
        //riferimento alla fase di gioco corrente
        public FasiDiGioco FasePartita { get { return _faseDiGioco; } set { _faseDiGioco = value; } }

        // Crea N=_numGiocatori giocatori. Utilizzabile solo prima che cominci la partita
        public void AggiungiGiocatori (int _numGiocatori)
        {
            //se la fase di gioco non è quella giusta o se ci sono già N giocatori si esce
            if (_faseDiGioco != FasiDiGioco.SceltaOpzioni || this._numGiocatori > _numGiocatori) throw new Exception(); 

            //aggiungo giocatori
            while (this._numGiocatori < _numGiocatori)
            {
                _giocatori.Add(new Giocatore());
                this._numGiocatori++;
            }
        }

        // Metodo per lanciare il setup della partita
        public void IniziaSetupPartita()
        {
           
            _faseDiGioco = FasiDiGioco.SetupPartita;
            foreach (Giocatore g in _giocatori)
            {
                //Interfaccia.ConsoleMessaggi.NuovoMessaggio("Setup del giocatore di colore" + g.Colore + " in corso", g.SpriteColor);
                g.GlobalInit();
                
            }
        }
       
        // Metodo per passare il turno al giocatore successivo
        public void NextTurn()
        {
            //se la partita non è in corso esce
            if (_faseDiGioco == FasiDiGioco.SceltaOpzioni) throw new Exception();
    

            //se la partita è in fase di setup lancio un evento di inizio partita
            if (_faseDiGioco == FasiDiGioco.SetupPartita)
            {
                if (++_contaTurni == _numGiocatori)
                {
                    _faseDiGioco = FasiDiGioco.PartitaInCorso;
                    InizioPartita(this, new EventArgs());
                    GiocatoreDiTurno.Init();
                    return;
                }
            }
            //se la partita è gia' cominciata
            else
            {
                GiocatoreDiTurno.Cleanup();
                _contaTurni++;
                GiocatoreDiTurno.Init();
            }
        }



    }

}
