using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

/// <summary>
/// Il tabellone in verità non esiste. 
/// E' formato da una serie di quadranti che vengono identificati con la posizione (ID_riga,ID_colonna) del centro del quadrante stesso.
/// Se ad esempio dico di disegnare il quadrante (4,4) la funzione provvederà a creare tutte le caselle che circondano quella posizione, 
/// oltre ovviamente alla casella stessa del centro.
/// In base alla posizione (ID_riga,ID_colonna) è in grado di capire quale texture usare.
/// 
/// ps: I numeriMagici = { 1, 4, 7, 10, 13, 16, 19 } sono le righe/colonne che contengono i pianeti (e quindi il centro dei quadranti)
/// in un tabellone con dimensione massima 7x7 quadranti (21x21 caselle).
/// </summary>

namespace Quantum_Game
{
    public class Tabellone
    {
        public Texture2D Texture { get; set; }
        int NumeroGiocatori { get; set; } // FORSE NON SERVE DAVVERO
        int NumeroQuadranti { get; set; } // FORSE NON SERVE DAVVERO


        public Tabellone(Texture2D texture, int numeroGiocatori, int numeroQuadranti)
        {
            Texture = texture;
            NumeroGiocatori = numeroGiocatori;
            NumeroQuadranti = numeroQuadranti;

        }

        List<Casella> listaCaselleAttive = new List<Casella>();
        
        public void DisegnaCaselleQuadrante(SpriteBatch spriteBatch, int larghCasella, int altCasella, int rigaQuad, int colQuad)
        {
            // SAREBBE MEGLIO SEPARARE LA PARTE CHE CREA GLI OGGETTI CASELLA CON IL RELATIVO TIPO E QUELLA CHE DISEGNA EFFETTIVAMENTE LA TEXTURE
            int tipoCas;

            //itero per tutte le caselle del quadrante, partendo dalla posizione centrale fornita
            for (int i = rigaQuad-1; i <= rigaQuad+1; i++)
            {
                for (int j = colQuad-1; j <= colQuad+1; j++)
                {
                    Console.WriteLine("rig,col = " + j +","+ i);
                    tipoCas = CalcolaTipoCasella(j, i);

                    //creo gli oggetti Casella e li metto in una lista, se non esistono ancora/già
                    if (!listaCaselleAttive.Any(x => x.ID_riga == i && x.ID_colonna == j)) 
                    {
                        listaCaselleAttive.Add(new Casella(i, j, tipoCas));
                    }

                    //la parte in cui disegna quello che ha calcolato
                    Rectangle tileDaDisegnare = new Rectangle(larghCasella * tipoCas, altCasella * 0, larghCasella, altCasella); // moltiplico per 0 perchè ho solo una riga nel Tileset
                    Rectangle doveLoDisegno = new Rectangle(i * larghCasella, j * altCasella, larghCasella, altCasella);

                    spriteBatch.Begin();
                    spriteBatch.Draw(Texture, doveLoDisegno, tileDaDisegnare, Color.White);
                    spriteBatch.End();

                }

            }
            Console.WriteLine("Numero caselle attive: " + listaCaselleAttive.Count);
        }

        public int CalcolaTipoCasella( int riga, int col)
        {
            //sfrutta la particolare struttura del tabellone per snellire il calcolo

            //int[] numeriMagici = { 1, 4, 7, 10, 13, 16, 19 };
            int Tipo = -1;

            // DEVO RISCRIVERLA USANDO UN FOREACH CHE ITERA TRA I NUMERI MAGICI
                while (Tipo < 0)
                {
                    if (riga == 1 || riga == 4 || riga == 7 || riga == 10 || riga == 13 || riga == 16 || riga == 19)
                    {
                        if (col == 1 || col == 4 || col == 7 || col == 10 || col == 13 || col == 16 || col == 19)
                        {
                            //Console.WriteLine("riga = Magico, colonna = Magico");
                            Tipo = 2; // pianeta
                        }
                        else
                        {
                            //Console.WriteLine("riga = Magico, colonna != Magico");
                            Tipo = 1; // orbita
                        }
                    }
                    else if (col == 1 || col == 4 || col == 7 || col == 10 || col == 13 || col == 16 || col == 19)
                    {
                        //Console.WriteLine("riga != Magico, colonna = Magico");
                        Tipo = 1; // orbita 
                    }
                    else
                    {
                        //Console.WriteLine("riga != Magico, colonna != Magico");
                        Tipo = 0; // vuota
                    }
            }

            //Console.WriteLine("Tipo della casella: " + Tipo);
            return Tipo;
        }






    }
}
