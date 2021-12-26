using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Util.Formula
{
    internal sealed class Element
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        private int qtde;

        public int QtdeParametros
        {
            get { return qtde; }
            set { qtde = value; }
        }
    }

    
}
