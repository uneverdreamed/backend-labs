// Базовый URL для работы с серверной сессией
const API = '/api/session';

// Сохранение данных в сессию на сервере
// Идентификатор сессии браузер получает в cookie Laba15.Session
async function save() {
    const name = document.getElementById('name').value;
    const group = document.getElementById('group').value;

    const response = await fetch(`${API}/save`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ name, group })
    });

    const data = await response.json();
    showResult(data);
}

// Чтение из сессии - сервер каждый раз увеличивает счётчик визитов
// Это показывает, что серверная сессия персистентна между запросами одного клиента
async function read() {
    const response = await fetch(`${API}/read`, {
        credentials: 'include'
    });
    const data = await response.json();
    showResult(data);
}

// Очистка сессии на сервере
async function clearData() {
    const response = await fetch(`${API}/clear`, {
        method: 'DELETE',
        credentials: 'include'
    });
    const data = await response.json();
    showResult(data);
}

function showResult(data) {
    document.getElementById('result').textContent = JSON.stringify(data, null, 2);
}