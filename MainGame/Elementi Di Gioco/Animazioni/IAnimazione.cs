using Microsoft.Xna.Framework;
namespace Quantum_Game.Animazioni
{
    public interface IAnimazione
    {
        /// <summary>
        /// Il corpo dell'animazione, chiamato ogni frame
        /// </summary>
        void Esegui();

        /// <summary>
        /// True quando l'animazione si è conclusa
        /// </summary>
        bool Completata { get; }

        /// <summary>
        /// Posizione della nave, aggiornata ad ogni frame
        /// </summary>
        Vector2 Posizione { get; }

        /// <summary>
        /// rotazione della nave, aggiornata ad ogni frame
        /// </summary>
        float Rotazione { get; }
    }
}
