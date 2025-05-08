import http from 'k6/http';
import { check } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 50 },   // Sobe até 50 usuários em 30s
        { duration: '1m', target: 100 },   // Sobe até 100 usuários em 1 min
        { duration: '1m', target: 200 },   // Sobe até 200 usuários em 1 min
        { duration: '30s', target: 0 },    // Finaliza
    ],
    thresholds: {
        http_req_failed: ['rate<0.05'],    // Até 5% de falhas é aceitável
        http_req_duration: ['p(95)<500'],  // 95% das respostas devem ser < 500ms
    },
};

export default function ()
{
    const res = http.get('http://localhost:5000/api/weatherforecast');
    check(res, {
        'status é 200': (r) => r.status === 200,
    });
}

