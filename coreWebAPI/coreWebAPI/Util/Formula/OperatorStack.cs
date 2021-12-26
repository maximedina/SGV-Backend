using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Util.Formula
{
    internal class OperatorStack : Stack<Operator>
    {
        int qtdeOPPrioridade50 = 0;
        int qtdeOPPrioridade100 = 0;
        int qtdeOPPrioridade200 = 0;

        public new Operator Pop()
        {
            return (Operator)base.Pop();
        }

        public new void Push(Operator op)
        {
            base.Push(op);
        }

        /// <summary>
        /// Esvazia a pilha de operadores jogando os elementos para a fila de execucao
        /// </summary>
        /// <param name="filaExec"></param>
        public void Empty(ref Queue<Element> filaExec)
        {
            while (this.Count > 0)
            {
                Operator opAux = this.Pop();
                Element e = new Element();
                e.Symbol = opAux.Simbolo;
                e.QtdeParametros = opAux.QtdeParametros;
                filaExec.Enqueue(e);
            }
            qtdeOPPrioridade50 = 0;
            qtdeOPPrioridade100 = 0;
            qtdeOPPrioridade200 = 0;
        }

        public void Push(Operator op, ref Queue<Element> filaExec)
        {
            if (op.Tipo == TipoOperador.LPAR)
            {
                this.Push(op);
            }
            else if (op.Tipo == TipoOperador.RPAR)
            {
                Operator opAux = new Operator("");
                while (this.Count > 0 && opAux.Tipo != TipoOperador.LPAR)
                {
                    opAux = this.Pop();
                    if (opAux.Tipo != TipoOperador.LPAR)
                    {
                        Element e = new Element();
                        e.Symbol = opAux.Simbolo;
                        e.QtdeParametros = opAux.QtdeParametros;
                        filaExec.Enqueue(e);
                    }
                }
            }
            else
            {
                switch (op.Prioridade)
                {
                    case PrioridadeOP.NORMAL:
                        qtdeOPPrioridade50++;
                        op.Prioridade -= qtdeOPPrioridade50;
                        break;
                    case PrioridadeOP.ALTA:
                        qtdeOPPrioridade100++;
                        op.Prioridade -= qtdeOPPrioridade100;
                        break;
                    case PrioridadeOP.MUITO_ALTA:
                        qtdeOPPrioridade200++;
                        op.Prioridade -= qtdeOPPrioridade200;
                        break;
                    default:
                        break;
                }

                while (this.Count > 0)
                {
                    Operator opAux = this.Peek();
                    if (op.Prioridade < opAux.Prioridade)
                    {
                        opAux = this.Pop();
                        Element e = new Element();
                        e.Symbol = opAux.Simbolo;
                        e.QtdeParametros = opAux.QtdeParametros;
                        filaExec.Enqueue(e);
                    }
                    else
                        break;
                }
                this.Push(op);
            }
        }
    }
}
