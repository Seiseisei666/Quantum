using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    /// <summary>
    /// semplicissima interfaccia dei bottoni:li obbliga ad avere una proprietà di sola lettura Check,
    /// che ci serve nel loop Update per vedere se sono stati cliccati
    /// </summary>
    public interface IBottone
    {
        bool Check();
        bottone TipoBottone { get; }
    }
}
