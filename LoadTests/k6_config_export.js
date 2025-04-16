// Configuração para exportar resultados do k6 para um arquivo ou ferramenta de monitoramento
import { textSummary } from 'https://jslib.k6.io/k6-summary/0.0.1/index.js';


export const options =
{
    scenarios: {
        default: {
            executor: 'constant-vus',
            vus: 10,
            duration: '10s',
        },
    },
};

// Função para exportar resultados para um arquivo local
export function handleSummary(data)
{
    // Obtém a data e hora atual
    const now = new Date();
    const timestamp = now.toISOString().replace(/[:.]/g, '-'); // Formata a data para evitar caracteres inválidos
    // Define o caminho e o nome do arquivo
    const filePath = './Resultados/resultados_teste_${timestamp}.json';
    return {
        // Salva os resultados em um arquivo JSON
        [filePath]: JSON.stringify(data), // Salva o arquivo no destino especificado
        stdout: textSummary(data, { indent: '  ' }), // Exibe o resumo no terminal

        // Para exportar para outras ferramentas, descomente as linhas abaixo
        //'resultados_teste_'+nomeTeste+timestamp+'.json': JSON.stringify(data), // Exporta os resultados em formato JSON
        //stdout: textSummary(data, { indent: '  ' }),  // Exibe um resumo no terminal
    };
}
