// JavaScript source code

import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 10, // n�mero de usu�rios virtuais
    duration: '10s', // tempo total do teste
};

export default function ()
{
    const res = http.get('https://localhost:7080/api/weatherforecast');

    check(res, {
        'status � 200': (r) => r.status === 200,
    });

    sleep(1); // simula tempo de espera entre requisi��es
}

