import http from 'k6/http';
import { check } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 50 },   // Sobe at� 50 usu�rios em 30s
        { duration: '1m', target: 100 },   // Sobe at� 100 usu�rios em 1 min
        { duration: '1m', target: 200 },   // Sobe at� 200 usu�rios em 1 min
        { duration: '30s', target: 0 },    // Finaliza
    ],
    thresholds: {
        http_req_failed: ['rate<0.05'],    // At� 5% de falhas � aceit�vel
        http_req_duration: ['p(95)<500'],  // 95% das respostas devem ser < 500ms
    },
};

export default function ()
{
    const res = http.get('http://localhost:5000/api/weatherforecast');
    check(res, {
        'status � 200': (r) => r.status === 200,
    });
}

