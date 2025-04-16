//Resumo do fluxo
//1.	O script configura 10 usuários virtuais(vus) que executam o teste por 10 segundos.
//2.	Cada usuário faz requisições GET para o endpoint https://localhost:7080/weatherforecast.
//3.	A resposta de cada requisição é validada para garantir que o status HTTP seja 200.
//4.	Após cada requisição, o usuário espera 1 segundo antes de repetir o processo..

//http: Este módulo fornece funções para realizar requisições HTTP(GET, POST, etc)
//check: Permite validar as respostas das requisições, verificando se atendem a critérios específicos(como status HTTP).
//sleep: Simula um tempo de espera entre as requisições, para imitar o comportamento e usuários reais.
// JavaScript source code
import http from 'k6/http';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import { handleSummary } from './k6_config_export.js'; // Importa a função handleSumma



// 🔐 Carrega usuários para login a partir de um arquivo JSON
const users = new SharedArray('usuarios', function ()
{
    return JSON.parse(open('./usuarios.json'));
});

// ⚠️ token se a rota exigir auth
const token = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6ImFkbWluIiwibmFtZWlkIjoiMTIzIiwiZW1haWwiOiJhZG1pbkBlbWFpbC5jb20iLCJqdGkiOiIzNGUxNjA3ZS1lMDgyLTQwMjktODExOS0xMTViOTU5YmM0YmIiLCJuYmYiOjE3NDQzNzg2NjIsImV4cCI6MTc0NDM4NTg2MiwiaWF0IjoxNzQ0Mzc4NjYyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDgwIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA4MCJ9.cAHjkGbQAV6kvp8-4Cuv_wjq95BY1K2iq55xbCjz0TA';

// 📌 Configura os 3 cenários com diferentes quantidades de usuários
export const options = {
    //metrics: {
    //    http_req_duration: { avg: 200, max: 500 },
    //}
    thresholds: {
        http_req_duration: ['avg<200', 'max<1000'], // Define limites para a métrica
    }, 
    scenarios: {
        login_users: {
            executor: 'constant-vus',
            exec: 'fazerLogin',
            vus: 2,
            duration: '10s',
        },
        previsao_users: {
            executor: 'constant-vus',
            exec: 'buscarPrevisao',
            vus: 5,
            duration: '10s',
        },
        cadastro_users: {
            executor: 'constant-vus',
            exec: 'cadastrarItem',
            vus: 1,
            duration: '10s',
        },
    }
};

// 🔑 Função de login
export function fazerLogin()
{
    const user = users[Math.floor(Math.random() * users.length)];
    const res = http.post('https://localhost:7080/auth/login', JSON.stringify({
        username: user.username,
        password: user.password,
    }), {
        headers: { 'Content-Type': 'application/json' },
        tags: { endpoint: 'login' },
    });

    check(res, {
        'status é 200': (r) => r.status === 200,
    });
   
    sleep(1);
}

// 🌤 Função para buscar previsão
export function buscarPrevisao()
{
    

    const res = http.get('https://localhost:7080/weatherforecast', {
        headers: { Authorization: token },
        tags: { endpoint: 'weatherforecast' },
    });

    check(res, {
        'status é 200': (r) => r.status === 200,
    });
   
    sleep(1);
}

// 📝 Função para cadastrar item
export function cadastrarItem()
{
   

    const item = {
        nome: `Item ${Math.random().toString(36).substring(7)}`,
        preco: Math.floor(Math.random() * 100),
    };

    const res = http.post('https://localhost:7080/items', JSON.stringify(item), {
        headers: {
            'Content-Type': 'application/json',
            Authorization: token,
        },
        tags: { endpoint: 'cadastro' },
    });

    check(res, {
        'status é 201 ou 200': (r) => r.status === 201 || r.status === 200,
    });
 
    sleep(2);
}
