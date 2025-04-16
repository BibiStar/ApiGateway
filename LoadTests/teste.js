// JavaScript source code

//Resumo do fluxo
//1.	O script configura 10 usuários virtuais(vus) que executam o teste por 10 segundos.
//2.	Cada usuário faz requisições GET para o endpoint https://localhost:7080/weatherforecast.
//3.	A resposta de cada requisição é validada para garantir que o status HTTP seja 200.
//4.	Após cada requisição, o usuário espera 1 segundo antes de repetir o processo.


//http: Este módulo fornece funções para realizar requisições HTTP(GET, POST, etc)
//check: Permite validar as respostas das requisições, verificando se atendem a critérios específicos(como status HTTP).
//sleep: Simula um tempo de espera entre as requisições, para imitar o comportamento e usuários reais.
import http from 'k6/http';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';


//requisições em loop por 10 segundos.Depois disso, o k6 espera até 30 segundos no máximo pra encerrar com calma as execuções pendentes(o graceful stop).
export const options = {
    vus: 10, // número de usuários virtuais
    duration: '10s', // tempo total do teste por usuário
    gracefulStop: '30s' // esse valor já é o padrão!
    
};
//1 cenário de teste que representa 100% da carga do teste
export default function ()
{
    const res = http.get('https://localhost:7080/weatherforecast', {
        headers: {
            Authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6ImFkbWluIiwibmFtZWlkIjoiMTIzIiwiZW1haWwiOiJhZG1pbkBlbWFpbC5jb20iLCJqdGkiOiIzNGUxNjA3ZS1lMDgyLTQwMjktODExOS0xMTViOTU5YmM0YmIiLCJuYmYiOjE3NDQzNzg2NjIsImV4cCI6MTc0NDM4NTg2MiwiaWF0IjoxNzQ0Mzc4NjYyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDgwIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA4MCJ9.cAHjkGbQAV6kvp8-4Cuv_wjq95BY1K2iq55xbCjz0TA'
        }
    });

    // Loga o status e parte do corpo da resposta para depuração
    console.log(`Status: ${res.status}`);
    console.log(`Body: ${res.body.substring(0, 100)}...`); // Evita imprimir um body inteiro gigante


    const ok=check(res, {
        'status é 200': (r) => r.status === 200,
    });

   
    if (!ok)
    {
        console.error(`❌ Requisição falhou com status ${res.status}`);
    }


   /* Faz com que cada usuário virtual espere 1 segundo antes de realizar a próxima iteração.Isso simula o comportamento de um usuário real, que não faz requisições consecutivas sem pausas.*/
    sleep(1); // simula tempo de espera entre requisições
    
}

