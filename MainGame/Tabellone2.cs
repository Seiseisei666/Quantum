using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Quantum_Game
{ 

    /// <summary>
    /// NON PIU' IN USO
    /// LO LASCIO SOLO PER VEDERE SE CASOMAI MI SONO PERSO QUALCOSA PER STRADA
    /// </summary>
    public class Tabellone2  
    {
        private int _righe, _colonne;
        public int Righe { get { return _righe; } }
        public int Colonne { get { return _colonne; } }
        public List<Zona> Tessere;

        public Tabellone2(int righe, int colonne)// : base (new Point (0,0), 1f, 50 * colonne, 50 * righe)
        {

            _righe = righe; _colonne = colonne;
            Tessere = new List<Zona>();
            Tessere.Capacity = righe * colonne;

          

           
        }

        /// <summary>
        /// inserisce un settore nel this (Da sinistra a destra, dall'alto verso il basso)
        /// Se il this è già pieno genera un errore!
        /// </summary>
        /// <param name="sec">Zona da inserire (Vuoto o Settore)</param>
        public void Popola (Zona sec)
        {
            if (this.Tessere.Count < this.Tessere.Capacity)
            {
               Tessere.Add(sec);
                return;
            }
            throw new Exception("Inseriti troppi settori nel tabellone!");
        }

        /// <summary>
        /// Metodo che prende come input la posizione relativa x y del mouse sul this (0...1)
        /// e restituisce l'indice del settore corrispondente (nella List Tessere)
        /// <returns>Restituisce l'indice intero a base 0 del settore nella Lista Tessere
        /// Se il punto non è contenuto nel tabellone restituisce un valore negativo!</returns>
        /// </summary> 
        public int CoordinateRelative2Sett(float xRel, float yRel)
        {
            if (xRel >= 1 || xRel < 0 || yRel >= 1 || yRel < 0) return -1; //restituisce -1 se il punto non è sul this
            float c = (float)_colonne; float r = (float)_righe;
            int iX, iY;
            iX = (int)Math.Floor((xRel * c));
            iY = ((int)Math.Floor((yRel * r))) * _colonne;

            return iX + iY;
        }

        public void CoordinatePixel2Casella (ref int x, ref int y)
        {
            decimal tempX = x; decimal tempY = y;
            //Debug.WriteLine(base.Superficie.Width);
       //       x = (int)Math.Floor((tempX / Larghezza) * this._colonne);
       //     y = (int)Math.Floor((tempY / Altezza) * this._righe);
             
            return;
          
        }
   

        /// <summary>
        /// Metodo che prende come input la posizione relativa x y del mouse sul this (0...1)
        /// e restituisce la posizione della casella nel settore (secondo l'enum e_posizione:
        /// 0 = alto a sin, 1 = alto, ecc
        /// </summary> 
        public e_posizione CoordinateRelative2Pos (float xRel, float yRel)
        {     
            float c = (float)_colonne; float r = (float)_righe;
            int iX, iY;
            iX = (int)(Math.Floor((xRel * c)) * 3) % 3;
            iY = (((int)Math.Floor((yRel * r))) * _colonne * 3) % 3;
            if (iX == 1 && iY == 1) return e_posizione.pianeta;
            return (e_posizione) iX + iY * 3;
        }

        /// <summary>
        /// Converte l'indice del settore (cioè la sua posizione nella lista Tessere) in valori relativi float
        /// </summary>
        /// <param name="Id">Indice del settore di cui si vuole conoscere la posizione nel this</param>
        /// <param name="x">parametro su cui scrivere il risultato x</param>
        /// <param name="y">parametro su cui scrivere il risultato y</param>
        public void IndiceSettore2xy (int Id, out float x, out float y)
        {
            int iX, iY;
            iX = Id % _colonne;
            iY = (Id / _colonne);
            x = (float)iX / (float)_colonne;
            y = (float)iY / (float)_righe;
            return;
        }

        /// Converte l'indice del settore (cioè la sua posizione nella lista Tessere) in valori in pixel (relativi all'origine del this)
        /// </summary>
        /// <param name="Id">Indice del settore di cui si vuole conoscere la posizione nel this</param>
        /// <param name="x">parametro su cui scrivere il risultato x</param>
        /// <param name="y">parametro su cui scrivere il risultato y</param>
        public void IndiceSettore2xy (int Id, out int x, out int y, int LarghezzaTabellonePixel, int AltezzaTabellonePixel)
        {
            int iX, iY;
            iX = Id % _colonne;
            iY = (Id / _colonne);
            x = (iX * LarghezzaTabellonePixel)/ _colonne;
            y = (iY *AltezzaTabellonePixel) / _righe;
            return;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch,Texture2D tileset, int ScreenH, int ScreenW)
        {
            //variabili varie che uso per disegnare i rettangoli
            int casW = 50;
            int casH = 50;
            int tabW = casW * this.Colonne * 3;
            int tabH = casH * this.Righe * 3;
            Rectangle target = new Rectangle();
            target.Width = casW;
            target.Height = casH;
            Rectangle source = new Rectangle();
            source.Height = 100;
            source.Width = 100;
            float x, y;
            int tempX, tempY;
      

            //loop per disegnare i settori
            for (int sett = 0; sett < this.Tessere.Count; sett++)
            {

                // il metodo indicesettore prende l'indice e mi rimanda le coordinate relative
                // di dove sta il settore in questione sulla plancia
                // (0,0 = angolo in alto a sin, 1,1 = angolo in basso a dx)
                this.IndiceSettore2xy(sett, out x, out y);
                x *= (float)tabW; y *= (float)tabH; // moltiplico queste coord relative
                                                    // per la dimensione del this che vogliamo disegnare

                for (int cas = 0; cas < 9; cas++)
                { //nested loop per disegnare le 9 caselle del settore

                    //offset delle 9 caselle (valori assoluti in pixel)
                    tempX = (cas % 3) * casW;
                    tempY = (cas / 3) * casH;

                    // se il settore è vuoto lo disegno vuoto e pace
                    if (this.Tessere[sett].Tipo == 0) { source.X = 0; source.Y = 0; }
                    else
                    {
                        switch (cas)
                        {

                            case 0:
                            case 2:
                            case 6:
                            case 8:
                                source.X = 0;
                                source.Y = 0;
                                break;
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                                source.X = 100;
                                source.Y = 0;
                                break;
                            default:
                                source.X = 200;
                                source.Y = 0;
                                break;
                        }

                    }



                    target.X = tempX + (int)x;
                    target.Y = tempY + (int)y;

                    //
                    // Debug.WriteLine("X Rettangolo: " + target.X + "Y Rettangolo: " + target.Y);
                    spriteBatch.Draw(tileset, target, source, Color.White);

                }

            }
        

        }


        /// <summary>
        /// Evento per processare il click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ClickSinistro (object sender, MouseEvntArgs args)

        {
            int tempX = args.Posizione.X;
            int tempY = args.Posizione.Y;
            Debug.WriteLine("Coordinate del click: {0}, {1}", tempX, tempY);
            this.CoordinatePixel2Casella(ref tempX, ref tempY);
            Debug.WriteLine("Coordinate casella: {0}, {1}", tempX, tempY );
        }

    }


}
