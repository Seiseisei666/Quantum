using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Quantum_Game
{
    /// <summary>
    /// FlussoDiGioco è un superoggettone che riunisce tutti i pezzi del gioco e li mette insieme.
    /// Non so se è buona prassi fare così... ma intanto ci ho provato
    /// </summary>
    public class FlussoDiGioco
    {
        private GameSystem gameSystem;
        private Tabellone tabellone;
        private Giocatore giocatoreDiTurno;
        private MouseInput mouseInput;
        private PathFinder pathFinder;

        private bool _leggiClickDx;
        public bool LeggiClickDx { get { return _leggiClickDx; } }


        public object OggettoSelezionato
        {
            get
            {
                Casella cas = tabellone.TileSelezionato as Casella;
                Pianeta pian = tabellone.TileSelezionato as Pianeta;

                if (cas != null && cas.Occupante != null)
                    return cas.Occupante;
                else if (pian != null)
                    return pian;
                else return null;
            }
        }
        private Nave _naveSel { get { return OggettoSelezionato as Nave; } }
        private Nave _naveSelOld;
        private Pianeta _pianetaSel { get { return OggettoSelezionato as Pianeta; } }
        private Casella _casellaSel { get { return tabellone.TileSelezionato as Casella; } }
        private Casella _casellaTarget { get { return tabellone.TileTarget as Casella; } }
        private Nave _naveTarget { get
            { 
                if (_casellaTarget != null)
                    return _casellaTarget.Occupante;
                else return null;
            }
        }
        private int _distanzaCasella { get { if (_casellaTarget != null)
                    return pathFinder.PercorsoXCasella(_casellaTarget).Length;
                else return 0; } }

        public FlussoDiGioco(GameSystem m_gameSystem, MouseInput m_mouseInput, Tabellone m_tabellone, PathFinder m_pathFinder)
        {
            gameSystem = m_gameSystem;
            tabellone = m_tabellone;
            mouseInput = m_mouseInput;
            pathFinder = m_pathFinder;
            _leggiClickDx = false;
            
        }

        public void Update()
        {
            giocatoreDiTurno = gameSystem.GiocatoreDiTurno;

            if (gameSystem.FasePartita == FasiDiGioco.PartitaInCorso)
            {
                /* QUI C'E' LA PARTITA VERA E PROPRIA!!!
                ***************************************/

                if (!Giocatore.PuòAgire())   // controlla se è finito il turno
                {
                    gameSystem.NextTurn();
                    Debug.WriteLine("Turno del giocatore {0}", gameSystem.GiocatoreDiTurno.Colore);
                }

                if (_naveSel != null && _naveSel.Alleato(giocatoreDiTurno))
                    pathFinder.Start(tabellone.TileSelezionato, _naveSel);
                else
                    pathFinder.Clear();
                if (_naveSel!= null && !_naveSel.Equals(_naveSelOld))
                    pathFinder.Clear();

                _naveSelOld = _naveSel;


                gestioneClickSinistro();
                if (_leggiClickDx)
                    gestioneClickDestro();


            }

            else if (gameSystem.FasePartita == FasiDiGioco.SetupPartita)
                setupPartita();
        }

        // Qui vengono fatte le operazioni relative al click sinistro
        // (per ora solo movimento/attacco)
        private void gestioneClickSinistro()
        {
            _leggiClickDx =                                 // condizioni per interessarsi al click DX:

                (_naveSel != null &&                        // nave selezionata AND
                _naveSel.Alleato(giocatoreDiTurno) &&       // alleata AND
                !(_naveSel.Mossa || _naveSel.SpecialUsata));  // la nave selezionata non ha mosso
        }

        // Se serve, qui gestiamo il click destro (movimento/attacco)
        private void gestioneClickDestro()
        {
            int dist = _distanzaCasella;
            
            if (dist == 0 || dist > _naveSel.Pwr )
                return;
            if (_naveTarget != null && !_naveTarget.Alleato(giocatoreDiTurno))
                {
                bool RisultatoAttacco;
                Debug.WriteLine("Una nave {0} di colore {1} ha attaccato una nave {2} di colore {3}.",
                            _naveSel.Tipo, _naveSel.Colore, _naveTarget.Tipo, _naveTarget.Colore);

                RisultatoAttacco = _naveSel.Attacco(_naveTarget);
                Debug.WriteLine("risultato: {0}", RisultatoAttacco);

                if (RisultatoAttacco == true)
                    _naveSel.Muovi(_casellaSel, _casellaTarget);
                }
            else if (_casellaTarget!= null) { 
                _naveSel.Muovi(_casellaSel, _casellaTarget);
                Debug.WriteLine(dist);
            }
        }

        // qui stiamo piazzando le pedine per il setup iniziale
        private void setupPartita ()
        {
            Casella tempCas = tabellone.TileSelezionato as Casella; // prova a castare il tile selezionato come casella
            Nave naveTemp = gameSystem.GiocatoreDiTurno.NaveDaPiazzare;
            if (naveTemp != null)
            {
                if (tempCas != null && tempCas.Occupante == null)
                    naveTemp.Piazza(tempCas);
            }
            else
                gameSystem.NextTurn();
        }

       }
}

