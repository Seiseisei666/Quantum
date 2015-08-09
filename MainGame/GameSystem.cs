using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public enum FasiDiGioco
    {
        SceltaOpzioni = -1,
        SetupPartita,
        PartitaInCorso
    }

    public class GameSystem
    {
        private static int _contaTurni;
        private List<Giocatore> _giocatori;
        private static FasiDiGioco _faseDiGioco;
        private int _numGiocatori;

        public static Dictionary<e_color, Color> QuantumColor;

        public int NumeroTurno { get { return _contaTurni; } }
        public FasiDiGioco FasePartita { get { return _faseDiGioco; } } //TOGLIERE IL SET E INTEGRARLO COL GIOCO

        public event EventHandler InizioPartita;

        public GameSystem()
        {
            _contaTurni = 0;
            _faseDiGioco = FasiDiGioco.SceltaOpzioni;

            QuantumColor = new Dictionary<e_color, Color>();
            QuantumColor.Add(e_color.Blu, Color.Blue);
            QuantumColor.Add(e_color.Rosso, Color.Red);
            QuantumColor.Add(e_color.Giallo, Color.Yellow);
            QuantumColor.Add(e_color.Verde, Color.Green);
        }

        /// <summary>
        /// crea N giocatori
        /// </summary>
        /// <param name="numeroGiocatori"></param>
        public void AggiungiGiocatori (int numeroGiocatori)
        {
            if (_faseDiGioco != FasiDiGioco.SceltaOpzioni) return; //se la fase di gioco non è quella giusta esce
            _giocatori = new List<Giocatore>();
            for (int i = 0; i < numeroGiocatori; i++)
                _giocatori.Add(new Giocatore());
            _numGiocatori = numeroGiocatori;
        }

        /// <summary>
        /// fornisce un riferimento al giocatore di turno
        /// </summary>
        public Giocatore GiocatoreDiTurno { get
            {
                return _giocatori[(_contaTurni % _giocatori.Count)]; }
            }

        public void IniziaSetupPartita()
        {
            _faseDiGioco = FasiDiGioco.SetupPartita;
            foreach (Giocatore g in _giocatori)
                g.GlobalInit();

        }

        public void NextTurn()
        {
            if (_faseDiGioco == FasiDiGioco.SceltaOpzioni) return; //se la partita non è in corso esce

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



    }
}
