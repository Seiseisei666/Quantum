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
        public static Dictionary<e_color, Color> QuantumColor = new Dictionary<e_color, Color>();

        public GestoreDiGiocatori()
        {
            giocatoreDiTurno = null;
            giocatori = new List<Giocatore>();

            //Definizione dei colori dei giocatore
            QuantumColor.Add(e_color.Blu, Color.Blue);
            QuantumColor.Add(e_color.Rosso, Color.Red);
            QuantumColor.Add(e_color.Giallo, Color.Yellow);
            QuantumColor.Add(e_color.Verde, Color.Green);
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
