A aplicação não foi desenvolvida com GraphQL devido a só ter trabalhado na questão de consulta e não na criação do webserice, o banco de dados usado foi o Mysql.<br/>
Foram acrescentados dois campos para informar sobre a execução do endpoint :<br/>
MsgStatus --> informa o status da requisição conforme enum KdMsgStatus;<br/>
         1 - sequência encontrada;<br/>
			   2 - sequência não encontrada;<br/>
			   3 - erro ao tentar encontrar a sequência;<br/>
			   4 - a pesquisa não retornou nenhum registro;<br/>
			 100 - erro interno no webservice;<br/>
Os erros podem ser acumulativos, pois podem ser apenas retorno bem como erros internos.<br/>
Ex.: A sequencia não foi encontrada "2" e deu erro na gravação dos dados "100", o resultado será: 2 + 100 = 102, ou seja, a sequência não foi encontrada e deu erro em algum outro método, a descrição dá a informação do método que gerou o erro.<br/><br/>
  			 			               
message   --> descrição do status;<br/>

Forma implementadas algumas validações nos endpoints.<br/>

Endereço para testes: http://audaces.nadlertecsass.com.br<br/>

Estou a disposição para esclarecer qualquer dúvida.
