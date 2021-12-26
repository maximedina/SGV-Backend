using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Util.Formula
{
    internal sealed class PrioridadeOP
    {
        public const int NORMAL = 50;
        public const int ALTA = 100;
        public const int MUITO_ALTA = 200;
    }

    internal sealed class TipoOperador
    {
        public const string NOP = "NOP";
        public const string MUL = "*";
        public const string ADD = "+";
        public const string SUB = "-";
        public const string DIV = "/";
        public const string MOD = "%";
        public const string LPAR = "(";
        public const string RPAR = ")";
        public const string ROOT = "ROOT";
        public const string GREATER = "MAIOR";
        public const string LESSER = "MENOR";
        public const string AVG = "MEDIA";
        public const string DESVP = "DESVIOP";
        public const string POW = "^";
        public const string TMEDIO = "TMEDIO()";
        public const string ABS = "ABS";
    }

    internal class Operator
    {
        private int _prioridade;
        private int _qtdeParametros;
        private string _simbolo;
        private string _tipo;

        #region CONSTRUTORES
        public Operator(string operador, int p_QuantidadeParams = 2)
        {
            this._qtdeParametros = p_QuantidadeParams;
            this._simbolo = operador;

            switch (operador.ToUpper())
            {
                case TipoOperador.ADD:
                    this._tipo = TipoOperador.ADD;
                    this._prioridade = PrioridadeOP.NORMAL;
                    break;
                case TipoOperador.SUB:
                    this._tipo = TipoOperador.SUB;
                    this._prioridade = PrioridadeOP.NORMAL;
                    break;
                case TipoOperador.MUL:
                    this._tipo = TipoOperador.MUL;
                    this._prioridade = PrioridadeOP.ALTA;
                    break;
                case TipoOperador.DIV:
                    this._tipo = TipoOperador.DIV;
                    this._prioridade = PrioridadeOP.ALTA;
                    break;
                case TipoOperador.POW:
                    this._tipo = TipoOperador.POW;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case TipoOperador.ROOT:
                    this._tipo = TipoOperador.ROOT;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case "MOD":
                    this._tipo = TipoOperador.MOD;
                    this._prioridade = PrioridadeOP.NORMAL;
                    break;
                case TipoOperador.LPAR:
                    this._tipo = TipoOperador.LPAR;
                    this._prioridade = 0;
                    break;
                case TipoOperador.RPAR:
                    this._tipo = TipoOperador.RPAR;
                    this._prioridade = 1;
                    break;
                case TipoOperador.GREATER:
                    this._tipo = TipoOperador.GREATER;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case TipoOperador.LESSER:
                    this._tipo = TipoOperador.LESSER;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case TipoOperador.AVG:
                    this._tipo = TipoOperador.AVG;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case TipoOperador.DESVP:
                    this._tipo = TipoOperador.DESVP;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                case TipoOperador.TMEDIO:
                    this._tipo = TipoOperador.TMEDIO;
                    this._prioridade = PrioridadeOP.MUITO_ALTA;
                    break;
                default:
                    this._tipo = TipoOperador.NOP;
                    this._simbolo = "";
                    break;

            }
        }
        #endregion //CONSTRUTORES
        #region PROPRIEDADES
        public int Prioridade
        {
            get { return _prioridade; }
            set { _prioridade = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
        }

        public string Simbolo
        {
            get { return _simbolo; }
            set { _simbolo = value; }
        }

        public int QtdeParametros
        {
            get { return _qtdeParametros; }
            set { _qtdeParametros = value; }
        }
        #endregion
    }
}
