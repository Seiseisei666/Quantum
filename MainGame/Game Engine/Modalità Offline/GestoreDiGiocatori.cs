using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    /* Classe che si occupa di creare e gestire i giocatori mantenendo traccia del giocatore attivo */
    public class GestoreDiGiocatori
    {
        /*Riferimento al giocatore di turno */
        private Giocatore giocatoreDiTurno;
        /* Giocatori partecipanti */
        private List<Giocatore> giocatori;

        // soluzione non molto elegante. Cmq converte i nostri colori nel formato System.Color
        public static readonly Dictionary<e_color, Color> QuantumColor = new Dictionary<e_color, Color>()
        {
            {e_color.Blu, Color.Blue},
            {e_color.Rosso, Color.Red },
            {e_color.Giallo, Color.Yellow },
            {e_color.Verde, Color.Green },
            {e_color.Arancione, Color.Orange },
            {e_color.Viola, Color.Purple },
            {e_color.Ciano, Color.Cyan },
            {e_color.Rosa, Color.Pink },
            {e_color.Bianco, Color.White },
            {e_color.Grigio, Color.Gray },
            {e_color.incolore, new Color(0x20,0x20,0x35)}
        };

        public GestoreDiGiocatori()
        {
            giocatoreDiTurno = null;
            giocatori = new List<Giocatore>();
        }

        /*riferimento alla lista di giocatori*/
        public List<Giocatore>  getGiocatori()
        {
            return giocatori;
        }

        /*riferimento al giocatore di turno */
        public Giocatore getGiocatoreDiTurno()
        {
            // USA LE PROPRIETA', MALEDETTO PROGRAMMATORE DI JAVA!!!
            return giocatoreDiTurno;
        }
        // public Giocatore GiocatoreDiTurno { get { return giocatoreDiTurno; } }

        // Aggiunge giocatori fino ad arrivare ad int numGiocatori
        //Costruttore implicito dei giocatori 
        public void creaGiocatori (int numGiocatori)
        {

            if (numGiocatori < 1 || numGiocatori > 4) throw new Exception();

            while (giocatori.Count < numGiocatori)
            {
                giocatori.Add(new Giocatore());
            }

            giocatoreDiTurno = giocatori[0];
         }
        /// <summary>
        /// Costruttore per N giocatori, ciascuno con un nome e un colore specifico
        /// </summary>
        /// <param name="nuoviGiocatori"></param>
        public void creaGiocatori (Dictionary<e_color, string> nuoviGiocatori)
        {
            if (nuoviGiocatori.Count < 2 || nuoviGiocatori.Count > 6) throw new ArgumentOutOfRangeException("giocatori");
            foreach (var g in nuoviGiocatori)
                giocatori.Add(new Giocatore(g.Value, g.Key));
            giocatoreDiTurno = giocatori[0];

        }

        /* Metodo che aggiorna il giocatore attivo */
        public void aggiornaGiocatoreDiTurno()
        {
            int index = giocatori.IndexOf(giocatoreDiTurno);
            index = (index + 1) % giocatori.Count;
            giocatoreDiTurno = giocatori[index];
            giocatoreDiTurno.Init();
        } 
      }
}
