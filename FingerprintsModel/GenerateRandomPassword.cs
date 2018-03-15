using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class GenerateRandomPassword
    {
        public static string GenerateRandomCode(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ12334560";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
