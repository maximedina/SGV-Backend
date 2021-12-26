using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MOM.Core.Util.Formulae
{
    enum STATES
    {
        S, VAR, NUM, NEG, EXP, REAL, DEC, ERRO, ROOT, ROOTVAR, ROOTNUM, ROOTNUMAUX, MAIOR_MENOR, MAIOR_MENOR_VAR, MAIOR_MENOR_NUM,
        MEDIA, MEDIA_VAR, MEDIA_NUM, DESVP, DESVP_VAR, DESVP_NUM, TMEDIO_AUX, TMEDIO_AUX2
    };

    public class Automaton
    {
        private STATES currentState;
        private const string NUM = "0123456789";
        private const string LETTER = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ_";
        private const string OPERATOR = "+-/*^%";
        Stack<string> bracketStack;
        OperatorStack operatorStack;
        int qtdeParamsComandoMaiorMenor = 1;
        Queue<Element> executionQueue;
        private string v_aux, n_aux;
        public System.Collections.ArrayList commandVariables;
        public System.Collections.ArrayList variables;

        public Automaton()
        {
            
            this.currentState = STATES.S; //INICIAL
            this.bracketStack = new Stack<string>();
            this.operatorStack = new OperatorStack();
            this.executionQueue = new Queue<Element>();
            this.variables = new System.Collections.ArrayList();
            this.commandVariables = new System.Collections.ArrayList();
            this.v_aux = "";
            this.n_aux = "";
        }

        private void changeState(string expression, int index)
        {
            if (index >= expression.Length)
                return;

            string token = expression[index].ToString();

            switch (this.currentState)
            {
                case STATES.S:
                    #region Inicial
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.NUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.VAR;
                        v_aux += token;
                    }
                    else if (token.Equals("("))
                    {
                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(token), ref executionQueue);
                        this.currentState = STATES.S;
                    }
                    else if (token.Equals("-"))
                    {
                        this.currentState = STATES.NEG;
                        n_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;

                case STATES.NEG:
                    #region NEG
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.NUM;
                        n_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;

                case STATES.VAR:
                    #region VAR
                    if (NUM.Contains(token) || LETTER.Contains(token))
                    {
                        this.currentState = STATES.VAR;
                        v_aux += token;
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("ROOT"))
                    {
                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.ROOT), ref executionQueue);
                        this.currentState = STATES.ROOT;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("MEDIA"))
                    {
                        int i = index;
                        qtdeParamsComandoMaiorMenor = 1;
                        while (expression[i] != ')' && i < expression.Length)
                        {
                            if (expression[i++] == ',')
                                qtdeParamsComandoMaiorMenor++;
                        }

                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.AVG, qtdeParamsComandoMaiorMenor), ref executionQueue);
                        this.currentState = STATES.MEDIA;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("DESVIOP"))
                    {
                        int i = index;
                        qtdeParamsComandoMaiorMenor = 1;
                        while (expression[i] != ')' && i < expression.Length)
                        {
                            if (expression[i++] == ',')
                                qtdeParamsComandoMaiorMenor++;
                        }

                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.DESVP, qtdeParamsComandoMaiorMenor), ref executionQueue);
                        this.currentState = STATES.DESVP;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("TMEDIO"))
                    {
                        operatorStack.Push(new Operator(TipoOperador.TMEDIO, 0), ref executionQueue);
                        this.currentState = STATES.TMEDIO_AUX;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("MAIOR"))
                    {
                        int i = index;
                        qtdeParamsComandoMaiorMenor = 1;
                        while (expression[i] != ')' && i < expression.Length)
                        {
                            if(expression[i++] == ',')
                                qtdeParamsComandoMaiorMenor++;
                        }

                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.GREATER, qtdeParamsComandoMaiorMenor), ref executionQueue);
                        this.currentState = STATES.MAIOR_MENOR;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("MENOR"))
                    {
                        int i = index;
                        qtdeParamsComandoMaiorMenor = 1;
                        while (expression[i] != ')' && i < expression.Length)
                        {
                            if (expression[i++] == ',')
                                qtdeParamsComandoMaiorMenor++;
                        }

                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.LESSER, qtdeParamsComandoMaiorMenor), ref executionQueue);
                        this.currentState = STATES.MAIOR_MENOR;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        this.currentState = STATES.VAR;
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            v_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else if (OPERATOR.Contains(token))
                    {
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            v_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                        this.currentState = STATES.EXP;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                    }
                    #endregion
                    break;
                case STATES.NUM:
                    #region NUM
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.NUM;
                        n_aux += token;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        this.currentState = STATES.NUM;
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else if (OPERATOR.Contains(token))
                    {
                        this.currentState = STATES.EXP;
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else if (token == ".")
                    {
                        this.currentState = STATES.REAL;
                        n_aux += token;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                        n_aux = "";
                    }
                    #endregion
                    break;
                case STATES.REAL:
                    #region REAL
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.DEC;
                        n_aux += token;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                        n_aux = "";
                    }
                    #endregion
                    break;
                case STATES.DEC:
                    #region DECIMAL
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.DEC;
                        n_aux += token;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        this.currentState = STATES.DEC;
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else if (OPERATOR.Contains(token))
                    {
                        this.currentState = STATES.EXP;
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                        n_aux = "";
                    }
                    #endregion
                    break;
                case STATES.EXP:
                    #region EXPRESSAO
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.NUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.VAR;
                        v_aux += token;
                    }
                    else if (token.Equals("("))
                    {
                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(token), ref executionQueue);
                        this.currentState = STATES.S;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                        n_aux = "";
                    }
                    #endregion
                    break;
                case STATES.ROOT:
                    #region RAIZ
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.ROOTNUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.ROOTVAR;
                        v_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;
                case STATES.ROOTVAR:
                    #region ROOTVAR
                    if (NUM.Contains(token) || LETTER.Contains(token))
                    {
                        this.currentState = STATES.ROOTVAR;
                        v_aux += token;
                    }
                    else if (token == ",")
                    {
                        this.currentState = STATES.ROOTNUMAUX;
                        if (v_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            v_aux = "";
                        }
                    }
                    else if (token == "(" && v_aux.ToUpper().Equals("ROOT"))
                    {
                        bracketStack.Push(token);
                        operatorStack.Push(new Operator(TipoOperador.ROOT), ref executionQueue);
                        this.currentState = STATES.ROOT;
                        n_aux = ""; v_aux = "";
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        this.currentState = STATES.ROOT;
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            v_aux = "";
                        }
                        operatorStack.Push(new Operator(token), ref executionQueue);
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                    }
                    #endregion
                    break;
                case STATES.ROOTNUMAUX:
                    #region ROOTNUMMAX
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.ROOTNUM;
                        n_aux += token;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                    }
                    #endregion
                    break;
                case STATES.ROOTNUM:
                    #region ROOTNUM
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.ROOTNUM;
                        n_aux += token;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                    {
                        this.currentState = STATES.ERRO;
                    }
                    #endregion
                    break;
                case STATES.MAIOR_MENOR:
                    #region MAIOR_MENOR
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.MAIOR_MENOR_NUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.MAIOR_MENOR_VAR;
                        v_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;
                case STATES.MAIOR_MENOR_NUM:
                    #region MAIOR_MENOR_NUM
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.MAIOR_MENOR_NUM;
                        n_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.MAIOR_MENOR;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;
                case STATES.MAIOR_MENOR_VAR:
                    #region MAIOR_MENOR_VAR
                    if (LETTER.Contains(token) || NUM.Contains(token))
                    {
                        this.currentState = STATES.MAIOR_MENOR_VAR;
                        v_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!v_aux.ToUpper().Contains("_ALL"))
                            {
                                throw new Exception("Variáveis dentro do comando MAIOR/MENOR/DESVIOP devem conter o sufixo _ALL em seu nome.");
                            }

                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            this.commandVariables.Add(v_aux);
                            v_aux = "";
                        }
                        this.currentState = STATES.MAIOR_MENOR;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!v_aux.ToUpper().Contains("_ALL"))
                            {
                                throw new Exception("Variáveis dentro do comando MAIOR/MENOR/DESVIOP devem conter o sufixo _ALL em seu nome.");
                            }
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }

                            foreach (var item in this.commandVariables)
                            {
                                if (!this.variables.Contains(item))
                                {
                                    this.variables.Add(item.ToString());
                                }
                            }
                            this.commandVariables.Clear();
                            v_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;

                case STATES.MEDIA:
                    #region MEDIA
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.MEDIA_NUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.MEDIA_VAR;
                        v_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.MEDIA_NUM:
                    #region MEDIA_NUM
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.MEDIA_NUM;
                        n_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.MEDIA;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.MEDIA_VAR:
                    #region MEDIA_VAR
                    if (LETTER.Contains(token) || NUM.Contains(token))
                    {
                        this.currentState = STATES.MEDIA_VAR;
                        v_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (v_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            this.commandVariables.Add(v_aux);
                            v_aux = "";
                        }
                        this.currentState = STATES.MEDIA;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (v_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }

                            foreach (var item in this.commandVariables)
                            {
                                if (!this.variables.Contains(item))
                                {
                                    this.variables.Add(item.ToString());
                                }
                            }
                            this.commandVariables.Clear();
                            v_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.DESVP:
                    #region DESVIO PADRAO
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.DESVP_NUM;
                        n_aux += token;
                    }
                    else if (LETTER.Contains(token))
                    {
                        this.currentState = STATES.DESVP_VAR;
                        v_aux += token;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.TMEDIO_AUX:
                    #region TAMANHO MEDIO DE GRAO
                    if (token == ")")
                    {
                        this.currentState = STATES.TMEDIO_AUX2;
                        this.variables.Add("TMEDIO()");
                        n_aux = ""; v_aux = "";
                    }
                    else 
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;

                case STATES.TMEDIO_AUX2:
                    if (OPERATOR.Contains(token))
                    {
                        operatorStack.Push(new Operator(token), ref executionQueue);
                        this.currentState = STATES.EXP;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    break;

                case STATES.DESVP_NUM:
                    #region DESVP_NUM
                    if (NUM.Contains(token))
                    {
                        this.currentState = STATES.DESVP_NUM;
                        n_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.DESVP;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (n_aux.Trim().Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                            n_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.DESVP_VAR:
                    #region DESVP_VAR
                    if (LETTER.Contains(token) || NUM.Contains(token))
                    {
                        this.currentState = STATES.DESVP_VAR;
                        v_aux += token;
                    }
                    else if (token == ",")
                    {
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!v_aux.ToUpper().Contains("_ALL"))
                            {
                                throw new Exception("Variáveis dentro do comando MAIOR/MENOR/DESVIOP devem conter o sufixo _ALL em seu nome.");
                            }

                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            this.commandVariables.Add(v_aux);
                            v_aux = "";
                        }
                        this.currentState = STATES.DESVP;
                    }
                    else if (token == ")")
                    {
                        bracketStack.Pop();
                        if (v_aux.Trim().Length > 0)
                        {
                            if (!v_aux.ToUpper().Contains("_ALL"))
                            {
                                throw new Exception("Variáveis dentro do comando MAIOR/MENOR/DESVIOP devem conter o sufixo _ALL em seu nome.");
                            }

                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);

                            if (!this.variables.Contains(v_aux))
                            {
                                this.variables.Add(v_aux);
                            }

                            foreach (var item in this.commandVariables)
                            {
                                if (!this.variables.Contains(item))
                                {
                                    this.variables.Add(item.ToString());
                                }
                            }
                            this.commandVariables.Clear();
                            v_aux = "";
                        }
                        this.currentState = STATES.VAR;
                    }
                    else
                        this.currentState = STATES.ERRO;
                    #endregion
                    break;


                case STATES.ERRO:
                    variables.Clear();
                    executionQueue.Clear();
                    operatorStack.Clear();
                    this.n_aux = "";
                    this.v_aux = "";
                    break;
            }

        }

        public bool validateSyntax(string formula)
        {
            this.currentState = STATES.S;
            formula = formula.Replace(" ", "");
            bracketStack.Clear();
            executionQueue.Clear();
            operatorStack.Clear();
            variables.Clear();
            this.v_aux = "";
            this.n_aux = "";

            try
            {
                for (int i = 0; i < formula.Length; i++)
                    this.changeState(formula, i);

                if (bracketStack.Count == 0)
                {
                    if ((currentState == STATES.VAR))
                    {
                        if (v_aux.Length > 0)
                        {
                            this.variables.Add(v_aux);
                            Element e = new Element();
                            e.Symbol = v_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                        }
                        operatorStack.Empty(ref executionQueue);
                        return true;
                    }
                    else if (currentState == STATES.NUM || currentState == STATES.DEC)
                    {
                        if (n_aux.Length > 0)
                        {
                            Element e = new Element();
                            e.Symbol = n_aux;
                            e.QtdeParametros = 0;
                            executionQueue.Enqueue(e);
                        }
                        operatorStack.Empty(ref executionQueue);
                        return true;
                    }
                    else if(currentState == STATES.TMEDIO_AUX2)
                    {
                        operatorStack.Empty(ref executionQueue);
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            }
            catch { return false; }
        }

        public double evaluate(System.Collections.Hashtable valuesByVariables, int precision = 3)
        {
            Stack<double> calculationStack = new Stack<double>();
            while (executionQueue.Count > 0)
            {
                Element item = executionQueue.Dequeue();

                if (OPERATOR.Contains(item.Symbol) || item.Symbol.ToUpper() == TipoOperador.ROOT || 
                    item.Symbol.ToUpper() == TipoOperador.GREATER || item.Symbol.ToUpper() == TipoOperador.LESSER ||
                    item.Symbol.ToUpper() == TipoOperador.AVG || item.Symbol.ToUpper() == TipoOperador.DESVP)
                {
                    doOperation(item, ref calculationStack);
                }
                else
                {
                    try
                    {
                        if (item.Symbol.Length == 0)
                        {
                            return Double.MinValue;
                        }
                        else
                        {
                            if (valuesByVariables.ContainsKey(item.Symbol))
                            {
                                item.Symbol = Convert.ToString(valuesByVariables[item.Symbol]);
                            }
                        }

                        item.Symbol = item.Symbol.Replace('.', ',');
                        if (String.IsNullOrWhiteSpace(item.Symbol))
                        {
                            return Double.MinValue;
                        }

                        calculationStack.Push(Convert.ToDouble(item.Symbol, System.Threading.Thread.CurrentThread.CurrentCulture));
                    }
                    catch
                    {
                        return Double.MinValue;
                    }
                }
            }
            return Math.Round(calculationStack.Peek(), precision, MidpointRounding.AwayFromZero);
        }


        private void doOperation(Element element, ref Stack<double> calculationStack)
        {
            switch (element.Symbol)
            {
                case TipoOperador.ADD:
                    calculationStack.Push(calculationStack.Pop() + calculationStack.Pop());
                    break;
                case TipoOperador.SUB:
                    double subtraendo = calculationStack.Pop();
                    double minuendo = calculationStack.Pop();
                    calculationStack.Push(minuendo - subtraendo);
                    break;
                case TipoOperador.MUL:
                    double x = calculationStack.Pop();
                    double y = calculationStack.Pop();
                    calculationStack.Push(x*y);
                    break;
                case TipoOperador.DIV:
                    double divisor = calculationStack.Pop();
                    double dividendo = calculationStack.Pop();
                    calculationStack.Push(dividendo / divisor);
                    break;
                case TipoOperador.POW:
                    double expoente = calculationStack.Pop();
                    double baseP = calculationStack.Pop();
                    calculationStack.Push(Math.Pow(baseP, expoente));
                    break;
                case TipoOperador.MOD:
                    double divisorMOD = calculationStack.Pop();
                    double dividendoMOD = calculationStack.Pop();
                    calculationStack.Push(dividendoMOD % divisorMOD);
                    break;
                case TipoOperador.ROOT:
                    double indice = calculationStack.Pop();
                    double radicando = calculationStack.Pop();
                    calculationStack.Push(Math.Pow(radicando, (1 / indice)));
                    break;
                case TipoOperador.GREATER:
                    List<double> listaNumeros = new List<double>();
                    double maiorNum = Double.MinValue;
                    for (int i = 0; i < element.QtdeParametros; i++)
                    {
                        listaNumeros.Add(calculationStack.Pop());
                    }

                    for (int i = 0; i < listaNumeros.Count; i++)
                    {
                        if (listaNumeros[i] > maiorNum)
                        {
                            maiorNum = listaNumeros[i];
                        }
                    }
                    
                    calculationStack.Push(maiorNum);
                    break;
                case TipoOperador.LESSER:
                    List<double> listaNumeros2 = new List<double>();
                    double menorNum = Double.MaxValue;
                    for (int i = 0; i < element.QtdeParametros; i++)
                    {
                        listaNumeros2.Add(calculationStack.Pop());
                    }

                    for (int i = 0; i < listaNumeros2.Count; i++)
                    {
                        if (listaNumeros2[i] < menorNum)
                        {
                            menorNum = listaNumeros2[i];
                        }
                    }

                    calculationStack.Push(menorNum);
                    break;

                case TipoOperador.AVG:
                    if (element.QtdeParametros > 0)
                    {
                        List<double> listaNumeros3 = new List<double>();
                        double soma = 0;
                        for (int i = 0; i < element.QtdeParametros; i++)
                        {
                            listaNumeros3.Add(calculationStack.Pop());
                        }

                        for (int i = 0; i < listaNumeros3.Count; i++)
                        {
                            soma += listaNumeros3[i];
                        }

                        calculationStack.Push(soma / element.QtdeParametros);
                    }
                    else
                    {
                        throw new Exception("Não foi especificado nenhum parâmetro para o comando MEDIA");
                    }
                    break;

                default:
                    throw (new Exception("Operador " + element.Symbol.ToString() + " não implementado."));
            }
        }
    }
}
