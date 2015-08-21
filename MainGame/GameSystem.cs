using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public enum FasiDiGioco
    {
        MenuIniziale = -1,
        SetupPartita,
        PartitaInCorso
    }
    /// <summary>
    /// Con questa classe gestiamo le fasi di gioco, i turni e i giocatori della partita
    /// Importanti la proprietà GiocatoreDiTurno (l'unico riferimento pubblico ai giocatori)
    /// e quella FasePartita.
    /// Invia un evento InizioPartita per informare tutto il programma che si comincia a giocare
    /// </summary>
    public class GameSystem
    {

        // CAMPI
        private static int _contaTurni;
        private List<Giocatore> _giocatori;
        private static FasiDiGioco _faseDiGioco;
        private int _numGiocatori;

        // COSTRUTTORE
        public GameSystem()
        {
            _contaTurni = _numGiocatori = 0;
            _faseDiGioco = FasiDiGioco.MenuIniziale;

            QuantumColor = new Dictionary<e_color, Color>();
            QuantumColor.Add(e_color.Blu, Color.Blue);
            QuantumColor.Add(e_color.Rosso, Color.Red);
            QuantumColor.Add(e_color.Giallo, Color.Yellow);
            QuantumColor.Add(e_color.Verde, Color.Green);
        }

        // PROPRIETA' PUBBLICHE

        public Giocatore GiocatoreDiTurno { get { return _giocatori[(_contaTurni % _giocatori.Count)]; } }
        public FasiDiGioco FasePartita { get { return _faseDiGioco; } }
            // soluzione non molto elegante. Cmq converte i nostri colori nel formato System.Color
        public static Dictionary<e_color, Color> QuantumColor;
            // per sfizio, mai usato... toglibile
        public int NumeroTurno { get { return _contaTurni; } }

        // METODI PUBBLICI

        public event EventHandler InizioPartita;
            // Crea N giocatori. Utilizzabile solo prima che cominci la partita
        public void AggiungiGiocatori (int numeroGiocatori)
        {
            if (_faseDiGioco != FasiDiGioco.MenuIniziale || _numGiocatori > 0)
                return; //se la fase di gioco non è quella giusta esce
            _giocatori = new List<Giocatore>();
            for (int i = 0; i < numeroGiocatori; i++)
                _giocatori.Add(new Giocatore());
            _numGiocatori = numeroGiocatori;
        }
            // Per passare dalla scelta opzioni alla fase di setup partita
        public void IniziaSetupPartita()
        {
            //_faseDiGioco = FasiDiGioco.SetupPartita;
            foreach (Giocatore g in _giocatori)
                g.GlobalInit();
        }

        public void IniziaMenuIniziale()
        {
            _faseDiGioco = FasiDiGioco.MenuIniziale;
        }
        
        // Prossimo turno
        public void NextTurn()
        {
            if (_faseDiGioco == FasiDiGioco.MenuIniziale) return; //se la partita non è in corso esce

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

            else
            {
                GiocatoreDiTurno.Cleanup();
                _contaTurni++;
                GiocatoreDiTurno.Init();
            }
        }

        public void SettaFaseDiGioco(FasiDiGioco fase) { _faseDiGioco = fase; } 

    }
}
