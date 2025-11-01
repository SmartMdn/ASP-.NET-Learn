# Library Management API

REST API для управления библиотекой с трехслойной архитектурой, Entity Framework Core и SQLite.

## 📋 Технологии

- **ASP.NET Core 8.0** - веб-фреймворк
- **Entity Framework Core** - ORM (Code First подход)
- **SQLite** - база данных
- **Swagger/OpenAPI** - документация API
- **Docker** - контейнеризация

### Требования

- **Для локального запуска:** [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Для Docker:** [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

## Запуск

### Вариант 1: Docker Compose (рекомендуется)

```bash
# Клонировать репозиторий
git clone <repository-url>
cd AspNET_Learn

# Запустить контейнер
docker-compose up -d

# Проверить статус
docker-compose ps

# Открыть Swagger UI
# http://localhost:8080
```

**Управление:**
```bash
# Просмотр логов
docker-compose logs -f

# Остановка
docker-compose down

# Остановка с удалением данных
docker-compose down -v
```

---

### Вариант 2: Локальный запуск

```bash
# Клонировать репозиторий
git clone <repository-url>
cd AspNET_Learn

# Восстановить зависимости
dotnet restore

# Перейти в API проект
cd LibraryManagementAPI

# Запустить приложение
dotnet run

# Открыть Swagger UI
# http://localhost:5000 или https://localhost:5001
```

База данных SQLite создастся автоматически при первом запуске с seed-данными.

---

## Тестирование API

### Swagger UI
Swagger UI доступен по умолчанию:
- **Локально:** http://localhost:5000
- **Docker:** http://localhost:8080

---
## Примеры запросов

### Авторы

#### Получить всех авторов
```http
GET /api/authors
```

**Ответ (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Лев Толстой",
    "dateOfBirth": "1828-09-09T00:00:00"
  }
]
```

#### Получить авторов с количеством книг
```http
GET /api/authors/with-book-count
```

**Ответ (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Лев Толстой",
    "dateOfBirth": "1828-09-09T00:00:00",
    "bookCount": 2
  }
]
```

#### Поиск авторов
```http
GET /api/authors?searchTerm=Толстой
```

#### Получить автора по ID
```http
GET /api/authors/1
```

#### Создать автора
```http
POST /api/authors
Content-Type: application/json

{
  "name": "Александр Пушкин",
  "dateOfBirth": "1799-06-06"
}
```

**Ответ (201 Created):**
```json
{
  "id": 4,
  "name": "Александр Пушкин",
  "dateOfBirth": "1799-06-06T00:00:00"
}
```

#### Обновить автора
```http
PUT /api/authors/1
Content-Type: application/json

{
  "name": "Лев Николаевич Толстой",
  "dateOfBirth": "1828-09-09"
}
```

**Ответ (204 No Content)**

#### Удалить автора
```http
DELETE /api/authors/1
```

**Ответ (204 No Content)**

---

### Книги

#### Получить все книги
```http
GET /api/books
```

**Ответ (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Война и мир",
    "publishedYear": 1869,
    "authorId": 1,
    "authorName": "Лев Толстой"
  }
]
```

#### Фильтр по году публикации
```http
GET /api/books?publishedAfterYear=1900
```

#### Создать книгу
```http
POST /api/books
Content-Type: application/json

{
  "title": "Евгений Онегин",
  "publishedYear": 1833,
  "authorId": 1
}
```

**Ответ (201 Created):**
```json
{
  "id": 6,
  "title": "Евгений Онегин",
  "publishedYear": 1833,
  "authorId": 1,
  "authorName": "Александр Пушкин"
}
```

#### Обновить книгу
```http
PUT /api/books/1
Content-Type: application/json

{
  "title": "Война и мир (полное издание)",
  "publishedYear": 1869,
  "authorId": 1
}
```

**Ответ (204 No Content)**

#### Удалить книгу
```http
DELETE /api/books/1
```

**Ответ (204 No Content)**

---

## Примеры ошибок

### Валидация даты
```http
POST /api/authors
Content-Type: application/json

{
  "name": "Тест",
  "dateOfBirth": "2030-01-01"
}
```

**Ответ (400 Bad Request):**
```json
{
  "message": "Дата рождения не может быть в будущем"
}
```

### Несуществующий автор
```http
POST /api/books
Content-Type: application/json

{
  "title": "Тест",
  "publishedYear": 2020,
  "authorId": 999
}
```

**Ответ (400 Bad Request):**
```json
{
  "message": "Автор с ID 999 не найден"
}
```

### Ресурс не найден
```http
GET /api/authors/999
```

**Ответ (404 Not Found):**
```json
{
  "message": "Автор с ID 999 не найден"
}
```





