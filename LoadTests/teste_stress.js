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
    const res = http.get('https://localhost:7080/orders', {
        headers: { Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6ImFkbWluIiwibmFtZWlkIjoiMTIzIiwiZW1haWwiOiJhZG1pbkBlbWFpbC5jb20iLCJqdGkiOiJjNmExNDY2My0xOTkxLTQzYWUtODliMS1jZjcxM2Q3M2UwYjkiLCJuYmYiOjE3NDY2Mzc5NTcsImV4cCI6MTc0NjY0NTE1NywiaWF0IjoxNzQ2NjM3OTU3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDgwIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA4MCJ9.0jIOwyhVzPmBzrxElyJAxOATrhIMDnHU_7vpXUYJi9U` }
    });
    check(res, {
        'status é 200': (r) => r.status === 200,
    });
}

