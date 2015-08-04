using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame.Utilities;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{ 
    public class Tabellone2
    {
        private int _righe, _colonne;
        public int Righe { get { return _righe; } }
        public int Colonne { get { return _colonne; } }
        public List<Zona> Tessere;

        public Tabellone2(int righe, int colonne)
        {

            _righe = righe; _colonne = colonne;
            Tessere = new List<Zona>();
            Tessere.Capacity = righe * colonne;
        }

        /// <summary>
        /// inserisce un settore nel tabellone (Da sinistra a destra, dall'alto verso il basso)
        /// Se il tabellone è già pieno genera un errore!
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
        /// Metodo che prende come input la posizione relativa x y del mouse sul tabellone (0...1)
        /// e restituisce l'indice del settore corrispondente (nella List Tessere)
        /// <returns>Restituisce l'indice intero a base 0 del settore nella Lista Tessere
        /// Se il punto non è contenuto nel tabellone restituisce un valore negativo!</returns>
        /// </summary> 
        public int CoordinateRelative2Sett (float xRel, float yRel)
        {
            if (xRel >= 1 || xRel < 0 || yRel >= 1 || yRel < 0) return -1; //restituisce -1 se il punto non è sul tabellone
            float c = (float)_colonne; float r = (float)_righe;
            int iX, iY;
            iX = (int)Math.Floor((xRel * c));
            iY = ((int)Math.Floor((yRel * r))) * _colonne;

            return iX + iY;
        }

        /// <summary>
        /// Metodo che prende come input la posizione relativa x y del mouse sul tabellone (0...1)
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
        /// <param name="Id">Indice del settore di cui si vuole conoscere la posizione nel tabellone</param>
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

        /// Converte l'indice del settore (cioè la sua posizione nella lista Tessere) in valori in pixel (relativi all'origine del tabellone)
        /// </summary>
        /// <param name="Id">Indice del settore di cui si vuole conoscere la posizione nel tabellone</param>
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

    }


}
