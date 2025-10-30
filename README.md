# Library Management API

REST API для управления библиотекой (авторы и книги) с использованием Entity Framework Core и SQLite.

## Новое: Entity Framework Core

API теперь использует **Entity Framework Core** с подходом **Code First** и базой данных **SQLite**. 

**Основные возможности:**
- Связь один-ко-многим между Author и Book
- LINQ-запросы для работы с данными
- Миграции для управления схемой БД
- Seed-данные (начальные авторы и книги)
- Дополнительные endpoints для сложных запросов


## Требования

### Для локального запуска:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) или выше

### Для запуска в Docker:
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Инструкция по запуску

### Способ 1: Локальный запуск через .NET CLI

1. **Клонировать репозиторий**
   ```bash
   git clone https://github.com/yourusername/LibraryManagementAPI.git
   cd LibraryManagementAPI
   ```

2. **Восстановить зависимости**
   ```bash
   dotnet restore
   ```

3. **Применить миграции (создать БД)**
   ```bash
   cd LibraryManagementAPI
   dotnet ef database update
   ```

4. **Запустить приложение**
   ```bash
   dotnet run
   ```

5. **Открыть в браузере**
   - Swagger UI: http://localhost:5000
   - HTTPS: https://localhost:5001

### Способ 2: Запуск через IDE (Rider/Visual Studio)

1. Открыть решение `LibraryManagementAPI.csproj`
2. Нажать **F5** или кнопку **Run**
3. Swagger UI откроется автоматически

### Способ 3: Docker Compose (рекомендуется для production)

**Особенности:**
- ✅ Персистентное хранение БД в Docker volume
- ✅ Автоматическое применение миграций
- ✅ Health checks
- ✅ Данные сохраняются между перезапусками

1. **Запустить контейнер**
   ```bash
   cd LibraryManagementAPI
   docker-compose up -d
   ```

2. **Проверить статус**
   ```bash
   docker-compose ps
   ```

3. **Открыть в браузере**
   - API: http://localhost:8080
   - Swagger UI: http://localhost:8080

4. **Просмотреть логи**
   ```bash
   docker-compose logs -f
   ```

5. **Остановить контейнер**
   ```bash
   docker-compose down
   ```

6. **Остановить и удалить БД (осторожно!)**
   ```bash
   docker-compose down -v
   ```

### Способ 4: Docker напрямую

```bash
cd LibraryManagementAPI

# Собрать образ
docker build -t library-management-api .

# Создать volume для БД
docker volume create library-data

# Запустить контейнер с volume
docker run -d \
  -p 8080:8080 \
  -v library-data:/app/data \
  --name library-api \
  library-management-api

# Просмотреть логи
docker logs -f library-api

# Остановить контейнер
docker stop library-api
docker rm library-api
```

## Проверка работы

После запуска откройте в браузере:
- **Локально**: http://localhost:5000
- **Docker**: http://localhost:8080

Swagger UI откроется автоматически для тестирования API.

---

## Новые LINQ-запросы (LibraryQueries)

### 1. Получить авторов с количеством книг
**GET** `/api/libraryqueries/authors-with-book-count`

**Ожидаемый результат (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Лев Толстой",
    "dateOfBirth": "1828-09-09T00:00:00",
    "bookCount": 2
  },
  {
    "id": 2,
    "name": "Фёдор Достоевский",
    "dateOfBirth": "1821-11-11T00:00:00",
    "bookCount": 2
  }
]
```

### 2. Получить книги после указанного года
**GET** `/api/libraryqueries/books-after-year/2015`

**Ожидаемый результат (200 OK):**
```json
[
  {
    "id": 6,
    "title": "Современная книга",
    "publishedYear": 2020,
    "authorId": 1,
    "authorName": "Лев Толстой"
  }
]
```

### 3. Поиск авторов по имени
**GET** `/api/libraryqueries/search-authors?searchTerm=Толстой`

**Ожидаемый результат (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Лев Толстой",
    "dateOfBirth": "1828-09-09T00:00:00"
  }
]
```

---
## Примеры запросов для Swagger/Postman

### 1. Получить всех авторов
**GET** `/api/authors`

**Ожидаемый результат (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Лев Толстой",
    "dateOfBirth": "1828-09-09T00:00:00"
  },
  {
    "id": 2,
    "name": "Фёдор Достоевский",
    "dateOfBirth": "1821-11-11T00:00:00"
  },
  {
    "id": 3,
    "name": "Антон Чехов",
    "dateOfBirth": "1860-01-29T00:00:00"
  }
]
```

### 2. Создать нового автора
**POST** `/api/authors`

**Тело запроса:**
```json
{
  "name": "Александр Пушкин",
  "dateOfBirth": "1799-06-06"
}
```

**Ожидаемый результат (201 Created):**
```json
{
  "id": 4,
  "name": "Александр Пушкин",
  "dateOfBirth": "1799-06-06T00:00:00"
}
```

### 3. Обновить автора
**PUT** `/api/authors/1`

**Тело запроса:**
```json
{
  "name": "Лев Николаевич Толстой",
  "dateOfBirth": "1828-09-09"
}
```

**Ожидаемый результат (200 OK):**
```json
{
  "id": 1,
  "name": "Лев Николаевич Толстой",
  "dateOfBirth": "1828-09-09T00:00:00"
}
```

### 4. Получить все книги
**GET** `/api/books`

**Ожидаемый результат (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Война и мир",
    "publishedYear": 1869,
    "authorId": 1
  },
  {
    "id": 2,
    "title": "Анна Каренина",
    "publishedYear": 1877,
    "authorId": 1
  },
  {
    "id": 3,
    "title": "Преступление и наказание",
    "publishedYear": 1866,
    "authorId": 2
  }
]
```

### 5. Создать новую книгу
**POST** `/api/books`

**Тело запроса:**
```json
{
  "title": "Евгений Онегин",
  "publishedYear": 1833,
  "authorId": 4
}
```

**Ожидаемый результат (201 Created):**
```json
{
  "id": 6,
  "title": "Евгений Онегин",
  "publishedYear": 1833,
  "authorId": 4
}
```

### 6. Обновить книгу
**PUT** `/api/books/1`

**Тело запроса:**
```json
{
  "title": "Война и мир (полное издание)",
  "publishedYear": 1869,
  "authorId": 1
}
```

**Ожидаемый результат (200 OK):**
```json
{
  "id": 1,
  "title": "Война и мир (полное издание)",
  "publishedYear": 1869,
  "authorId": 1
}
```

### 7. Удалить книгу
**DELETE** `/api/books/1`

**Ожидаемый результат (204 No Content):**
```
Нет тела ответа
```

### Примеры ошибок валидации

**Создание автора с датой в будущем:**
```json
{
  "name": "Тестовый Автор",
  "dateOfBirth": "2030-01-01"
}
```

**Результат (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "DateOfBirth": [
      "Дата рождения не может быть в будущем"
    ]
  }
}
```

**Создание книги с несуществующим автором:**
```json
{
  "title": "Тестовая книга",
  "publishedYear": 2020,
  "authorId": 999
}
```

**Результат (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "AuthorId": [
      "Автор с ID 999 не найден"
    ]
  }
}
```





