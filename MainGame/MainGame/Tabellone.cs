using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public class Tabellone
    {
        public Texture2D Texture { get; set; }
        int NumeroGiocatori { get; set; }
        int NumeroQuadranti { get; set; }


        public Tabellone(Texture2D texture, int numeroGiocatori, int numeroQuadranti)
        {
            Texture = texture;
            NumeroGiocatori = numeroGiocatori;
            NumeroQuadranti = numeroQuadranti;

        }

        public void DisegnaCaselleQuadrante(SpriteBatch spriteBatch, int larghCasella, int altCasella, int rigaQuad, int colQuad)
        {
            int tipoCas;
            for (int i = rigaQuad-1; i <= rigaQuad+1; i++)
            {
                for (int j = colQuad-1; j <= colQuad+1; j++)
                {
                    Console.WriteLine("rig,col = " + j +","+ i);
                    tipoCas = CalcolaTipoCasella(j, i);
                    Rectangle tileDaDisegnare = new Rectangle(larghCasella * tipoCas, altCasella * 0, larghCasella, altCasella); // moltiplico per 0 perchè ho solo una riga nel Tileset
                    Rectangle doveLoDisegno = new Rectangle(i * larghCasella, j * altCasella, larghCasella, altCasella);

                    spriteBatch.Begin();
                    spriteBatch.Draw(Texture, doveLoDisegno, tileDaDisegnare, Color.White);
                    spriteBatch.End();

                }

            }
        }

        public int CalcolaTipoCasella( int riga, int col)
        {
            //int[] numeriMagici = { 1, 4, 7, 10, 13, 16, 19 };
            //int Magico;
            int Tipo = -1;
            int i = 0;

            //for (int i = 0; i <= numeriMagici.Length-1; i++)
            //{
                //while (Tipo < 0 && i <= numeriMagici.Length - 1)
                while (Tipo < 0)
                {
                    //Magico = numeriMagici[i];
                    //Console.WriteLine("i = " + i + ", Magico = " + Magico);
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
                    i++;
              //  }
            }

            //Console.WriteLine("Tipo della casella: " + Tipo);
            return Tipo;
        }






    }
}
