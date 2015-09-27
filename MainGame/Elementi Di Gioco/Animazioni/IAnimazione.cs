using Microsoft.Xna.Framework;
namespace Quantum_Game.Animazioni
{
    public interface IAnimazione
    {
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
