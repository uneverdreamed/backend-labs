// модуль дл€ работы с API Ч все HTTP-запросы к серверу собраны здесь
// используетс€ Fetch API дл€ отправки запросов и получени€ JSON-ответов

const API_BASE = '/api';

// универсальна€ функци€ дл€ выполнени€ запроса и обработки ответа
async function apiRequest(url, method = 'GET', body = null) {
    const options = {
        method: method,
        headers: { 'Content-Type': 'application/json' }
    };

    // тело запроса добавл€етс€ только дл€ POST и PUT
    if (body) {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(API_BASE + url, options);

    // дл€ 204 No Content тела нет Ч возвращаем null
    if (response.status === 204) {
        return { ok: true, data: null };
    }

    const data = await response.json();

    if (!response.ok) {
        // сервер вернул ошибку Ч извлекаем сообщение
        // формат может быть разный: от ValidationProblem (поле errors) до нашего {error: "..."}
        let errorMessage = data.error || data.title || 'Ќеизвестна€ ошибка';

        // если это ошибка валидации ModelState Ч собираем все сообщени€
        if (data.errors) {
            const messages = [];
            for (const field in data.errors) {
                messages.push(...data.errors[field]);
            }
            errorMessage = messages.join('; ');
        }

        return { ok: false, error: errorMessage };
    }

    return { ok: true, data: data };
}

// === —туденты ===
async function getStudents() {
    return apiRequest('/students');
}

async function createStudent(name, group) {
    return apiRequest('/students', 'POST', { name, group });
}

async function deleteStudent(id) {
    return apiRequest(`/students/${id}`, 'DELETE');
}

// ===  урсы ===
async function getCourses() {
    return apiRequest('/courses');
}

async function createCourse(name, description, credits) {
    return apiRequest('/courses', 'POST', { name, description, credits: parseInt(credits) });
}

async function deleteCourse(id) {
    return apiRequest(`/courses/${id}`, 'DELETE');
}

// === «аписи на курсы ===
async function getEnrollments() {
    return apiRequest('/enrollments');
}

async function createEnrollment(studentId, courseId) {
    return apiRequest('/enrollments', 'POST', {
        studentId: parseInt(studentId),
        courseId: parseInt(courseId)
    });
}

async function deleteEnrollment(id) {
    return apiRequest(`/enrollments/${id}`, 'DELETE');
}