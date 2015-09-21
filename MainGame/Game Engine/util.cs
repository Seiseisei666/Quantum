using System;
namespace Quantum_Game {
    /// <summary>
    /// Sta roba non si può vedere... è una delle prime cose che avevo fatto e fa davvero schifo
    /// c'è qualche volontario che la schiaffa da un'altra parte?
    /// </summary>
    public static class util {
        

private static Random _dado;
		
		 static util () {
			_dado = new Random();
		}
		
		public static int Dadi (int num = 1) {
			int res = 0;
			for (int i = 0; i < num; i++) {
				res += _dado.Next(1,7);
			}
            if (res > 6) throw new Exception("res");
			return res;
		}
	}}
	