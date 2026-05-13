// клиентский скрипт — отправляет кросс-доменные запросы к API на порту 5000
// и отображает результаты: успешные ответы, CORS-заголовки и заблокированные запросы

const API = 'http://localhost:5000/api';

// универсальная функция отправки запроса и отображения результата
async function sendRequest(url, method, body, resultId) {
    const resultEl = document.getElementById(resultId);
    resultEl.className = 'result show';
    resultEl.textContent = 'Отправка запроса...';

    try {
        const options = {
            method: method
        };
        // Content-Type нужен только для запросов с телом (POST, PUT)
        if (body) {
            options.headers = { 'Content-Type': 'application/json' };
            options.body = JSON.stringify(body);
        }

        const response = await fetch(url, options);

        let data = null;
        const text = await response.text();
        if (text) {
            data = JSON.parse(text);
        }

        // собираем CORS-заголовки из ответа для отображения
        const corsHeaders = [];
        const headerNames = [
            'access-control-allow-origin',
            'access-control-allow-methods',
            'access-control-allow-headers'
        ];
        headerNames.forEach(name => {
            const value = response.headers.get(name);
            if (value) corsHeaders.push(`${name}: ${value}`);
        });

        const headersText = corsHeaders.length > 0
            ? '\n\nCORS-заголовки ответа:\n' + corsHeaders.join('\n')
            : '\n\n(CORS-заголовки отсутствуют в ответе)';

        resultEl.className = response.ok ? 'result show success' : 'result show error';
        resultEl.textContent = `${response.status} ${response.statusText}\n`
            + JSON.stringify(data, null, 2)
            + headersText;

    } catch (err) {
        // сюда попадаем когда браузер заблокировал запрос из-за CORS
        // fetch выбрасывает TypeError при CORS-ошибке
        resultEl.className = 'result show blocked';
        resultEl.textContent = 'ЗАБЛОКИРОВАНО БРАУЗЕРОМ (CORS)\n\n'
            + 'Ошибка: ' + err.message + '\n\n'
            + 'Браузер не отправил запрос или отклонил ответ,\n'
            + 'потому что сервер не вернул разрешающий\n'
            + 'заголовок Access-Control-Allow-Origin для этого источника.';
    }
}

// === Тест 1: Students — политика "AllowClient" (localhost:5001 разрешён) ===

function testStudentsGet() {
    sendRequest(`${API}/students`, 'GET', null, 'result-students-get');
}

function testStudentsPost() {
    sendRequest(`${API}/students`, 'POST',
        { name: 'Тестовый Студент', group: '241-335' },
        'result-students-post');
}

function testStudentsDelete() {
    sendRequest(`${API}/students/1`, 'DELETE', null, 'result-students-delete');
}

// === Тест 2: PublicCourses — политика "ReadOnly" (GET с любого домена) ===

function testPublicGet() {
    sendRequest(`${API}/publiccourses`, 'GET', null, 'result-public-get');
}

function testPublicPost() {
    sendRequest(`${API}/publiccourses`, 'POST',
        { name: 'Новый курс', description: 'Тест', credits: 3 },
        'result-public-post');
}

// === Тест 3: Courses — DELETE с [DisableCors] ===

function testCoursesGet() {
    sendRequest(`${API}/courses`, 'GET', null, 'result-courses-get');
}

function testCoursesDelete() {
    sendRequest(`${API}/courses/1`, 'DELETE', null, 'result-courses-delete');
}