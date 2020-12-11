A aplicação não foi desenvolvida com GraphQL devido a só ter trabalhado na questão de consulta e não na criação do webserice, o banco de dados usado foi o Mysql.
Foram acrescentados dois campos para informar sobre a execução do endpoint :
MsgStatus --> informa o status da requisição conforme enum KdMsgStatus;
         1 - sequência encontrada;
			   2 - sequência não encontrada;
			   3 - erro ao tentar encontrar a sequência;
			   4 - a pesquisa não retornou nenhum registro;
			 100 - erro interno no webservice;
Os erros podem ser acumulativos, pois podem ser apenas retorno bem como erros internos.
Ex.: A sequencia não foi encontrada "2" e deu erro na gravação dos dados "100", o resultado será: 2 + 100 = 102, ou seja, a sequência não foi encontrada e deu erro em algum outro método, a descrição dá a informação do método que gerou o erro.
  			 			               
message   --> descrição do status;

Forma implementadas algumas validações nos endpoints.
