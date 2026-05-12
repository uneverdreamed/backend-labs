// основной скрипт приложения — управление вкладками, рендер данных, обработка форм

// === НАВИГАЦИЯ ПО ВКЛАДКАМ ===

// при клике на кнопку навигации показываем соответствующую секцию
document.querySelectorAll('nav button').forEach(btn => {
    btn.addEventListener('click', () => {
        const target = btn.dataset.section;

        // убираем класс active у всех кнопок и секций
        document.querySelectorAll('nav button').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));

        // активируем нужную кнопку и секцию
        btn.classList.add('active');
        document.getElementById(target).classList.add('active');

        // загружаем данные для выбранной вкладки
        loadSection(target);
    });
});

// === СООБЩЕНИЯ ===

// показать сообщение об успехе или ошибке, автоматически скрывается через 4 секунды
function showMessage(elementId, text, type) {
    const el = document.getElementById(elementId);
    el.textContent = text;
    el.className = `message show message-${type}`;
    setTimeout(() => el.classList.remove('show'), 4000);
}

// === ЗАГРУЗКА ДАННЫХ ===

async function loadSection(section) {
    if (section === 'students') await loadStudents();
    if (section === 'courses') await loadCourses();
    if (section === 'enrollments') await loadEnrollments();
}

// --- Студенты ---

async function loadStudents() {
    const tbody = document.getElementById('students-table');
    tbody.innerHTML = '<tr><td colspan="5" class="loading">Загрузка...</td></tr>';

    const result = await getStudents();

    if (!result.ok) {
        tbody.innerHTML = `<tr><td colspan="5" class="loading">${result.error}</td></tr>`;
        return;
    }

    if (result.data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" class="loading">Нет данных</td></tr>';
        return;
    }

    // рендерим строки таблицы из полученных данных
    tbody.innerHTML = result.data.map(s => `
        <tr>
            <td>${s.id}</td>
            <td>${s.name}</td>
            <td>${s.group}</td>
            <td>${new Date(s.createdAt).toLocaleDateString('ru-RU')}</td>
            <td class="actions">
                <button class="btn btn-danger btn-sm" onclick="onDeleteStudent(${s.id})">Удалить</button>
            </td>
        </tr>
    `).join('');
}

// обработчик формы создания студента
document.getElementById('student-form').addEventListener('submit', async (e) => {
    e.preventDefault(); // отменяем стандартную отправку формы

    const name = document.getElementById('student-name').value.trim();
    const group = document.getElementById('student-group').value.trim();

    const result = await createStudent(name, group);

    if (result.ok) {
        showMessage('student-msg', `Студент "${name}" добавлен`, 'success');
        e.target.reset(); // очищаем поля формы
        await loadStudents(); // обновляем таблицу
    } else {
        showMessage('student-msg', result.error, 'error');
    }
});

// удаление студента с подтверждением
async function onDeleteStudent(id) {
    if (!confirm('Удалить студента?')) return;

    const result = await deleteStudent(id);

    if (result.ok) {
        showMessage('student-msg', 'Студент удалён', 'success');
        await loadStudents();
    } else {
        showMessage('student-msg', result.error, 'error');
    }
}

// --- Курсы ---

async function loadCourses() {
    const tbody = document.getElementById('courses-table');
    tbody.innerHTML = '<tr><td colspan="5" class="loading">Загрузка...</td></tr>';

    const result = await getCourses();

    if (!result.ok) {
        tbody.innerHTML = `<tr><td colspan="5" class="loading">${result.error}</td></tr>`;
        return;
    }

    if (result.data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" class="loading">Нет данных</td></tr>';
        return;
    }

    tbody.innerHTML = result.data.map(c => `
        <tr>
            <td>${c.id}</td>
            <td>${c.name}</td>
            <td>${c.description}</td>
            <td>${c.credits}</td>
            <td class="actions">
                <button class="btn btn-danger btn-sm" onclick="onDeleteCourse(${c.id})">Удалить</button>
            </td>
        </tr>
    `).join('');

    // обновляем список курсов в select на вкладке записей
    updateCourseSelect(result.data);
}

document.getElementById('course-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const name = document.getElementById('course-name').value.trim();
    const description = document.getElementById('course-desc').value.trim();
    const credits = document.getElementById('course-credits').value;

    const result = await createCourse(name, description, credits);

    if (result.ok) {
        showMessage('course-msg', `Курс "${name}" добавлен`, 'success');
        e.target.reset();
        await loadCourses();
    } else {
        showMessage('course-msg', result.error, 'error');
    }
});

async function onDeleteCourse(id) {
    if (!confirm('Удалить курс?')) return;

    const result = await deleteCourse(id);

    if (result.ok) {
        showMessage('course-msg', 'Курс удалён', 'success');
        await loadCourses();
    } else {
        showMessage('course-msg', result.error, 'error');
    }
}

// --- Записи на курсы ---

async function loadEnrollments() {
    const tbody = document.getElementById('enrollments-table');
    tbody.innerHTML = '<tr><td colspan="6" class="loading">Загрузка...</td></tr>';

    const result = await getEnrollments();

    if (!result.ok) {
        tbody.innerHTML = `<tr><td colspan="6" class="loading">${result.error}</td></tr>`;
        return;
    }

    if (result.data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="loading">Нет данных</td></tr>';
        return;
    }

    tbody.innerHTML = result.data.map(e => `
        <tr>
            <td>${e.id}</td>
            <td>${e.studentId}</td>
            <td>${e.courseId}</td>
            <td>${new Date(e.enrolledAt).toLocaleDateString('ru-RU')}</td>
            <td>${e.grade ?? '—'}</td>
            <td class="actions">
                <button class="btn btn-danger btn-sm" onclick="onDeleteEnrollment(${e.id})">Удалить</button>
            </td>
        </tr>
    `).join('');

    // обновляем select студентов
    const studentsResult = await getStudents();
    if (studentsResult.ok) updateStudentSelect(studentsResult.data);
}

document.getElementById('enroll-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const studentId = document.getElementById('enroll-student').value;
    const courseId = document.getElementById('enroll-course').value;

    const result = await createEnrollment(studentId, courseId);

    if (result.ok) {
        showMessage('enroll-msg', 'Запись добавлена', 'success');
        e.target.reset();
        await loadEnrollments();
    } else {
        showMessage('enroll-msg', result.error, 'error');
    }
});

async function onDeleteEnrollment(id) {
    if (!confirm('Удалить запись?')) return;

    const result = await deleteEnrollment(id);

    if (result.ok) {
        showMessage('enroll-msg', 'Запись удалена', 'success');
        await loadEnrollments();
    } else {
        showMessage('enroll-msg', result.error, 'error');
    }
}

// заполнение выпадающих списков данными с сервера
function updateStudentSelect(students) {
    const select = document.getElementById('enroll-student');
    select.innerHTML = '<option value="">Выберите студента</option>'
        + students.map(s => `<option value="${s.id}">${s.name} (${s.group})</option>`).join('');
}

function updateCourseSelect(courses) {
    const select = document.getElementById('enroll-course');
    select.innerHTML = '<option value="">Выберите курс</option>'
        + courses.map(c => `<option value="${c.id}">${c.name}</option>`).join('');
}

// === НАЧАЛЬНАЯ ЗАГРУЗКА ===
// при открытии страницы загружаем первую вкладку
loadStudents();