## Отчет о выполнении Домашнего Задания №2

**Исполнитель:** Пасынков Матвей Евгеньевич БПИ237

Настоящий отчет описывает реализацию веб-приложения "Управление Зоопарком" в соответствии с требованиями.

### 1. Реализованный Функционал (Требование 6a, Требование 1)

Все основные Use Cases, указанные в задании, были реализованы в виде REST API эндпоинтов в слое представления (`ZooManagement.WebAPI`) и доступны для тестирования через Swagger.

*   **a. Добавить / удалить животное:**
    *   **Реализация:**
        *   `POST /api/animals`: Добавление нового животного.
        *   `DELETE /api/animals/{id}`: Удаление животного по ID.
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/AnimalsController.cs` (методы `CreateAnimal`, `DeleteAnimal`).
        *   Сервис Приложения: `ZooManagement.Application/Services/AnimalService.cs` (методы `AddAnimalAsync`, `DeleteAnimalAsync`).
*   **b. Добавить / удалить вольер:**
    *   **Реализация:**
        *   `POST /api/enclosures`: Добавление нового вольера.
        *   `DELETE /api/enclosures/{id}`: Удаление вольера по ID (с проверкой на пустоту).
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/EnclosuresController.cs` (методы `CreateEnclosure`, `DeleteEnclosure`).
        *   Сервис Приложения: `ZooManagement.Application/Services/EnclosureService.cs` (методы `AddEnclosureAsync`, `DeleteEnclosureAsync`).
*   **c. Переместить животное между вольерами:**
    *   **Реализация:**
        *   `POST /api/animals/{id}/transfer`: Перемещение животного с указанным ID в целевой вольер (ID вольера передается в теле запроса).
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/AnimalsController.cs` (метод `TransferAnimal`).
        *   Сервис Приложения: `ZooManagement.Application/Services/AnimalTransferService.cs` (метод `TransferAnimalAsync`).
*   **d. Просмотреть расписание кормления:**
    *   **Реализация:**
        *   `GET /api/feedingschedules`: Получение всех расписаний.
        *   `GET /api/feedingschedules/{id}`: Получение расписания по ID.
        *   `GET /api/feedingschedules/animal/{animalId}`: Получение всех расписаний для конкретного животного.
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/FeedingSchedulesController.cs` (методы `GetAllSchedules`, `GetScheduleById`, `GetSchedulesForAnimal`).
        *   Сервис Приложения: `ZooManagement.Application/Services/FeedingOrganizationService.cs` (методы `GetAllSchedulesAsync`, `GetScheduleByIdAsync`, `GetSchedulesForAnimalAsync`).
*   **e. Добавить новое кормление в расписание:**
    *   **Реализация:**
        *   `POST /api/feedingschedules`: Добавление нового расписания кормления (ID животного, время, тип пищи передаются в теле запроса).
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/FeedingSchedulesController.cs` (метод `CreateSchedule`).
        *   Сервис Приложения: `ZooManagement.Application/Services/FeedingOrganizationService.cs` (метод `AddFeedingScheduleAsync`).
*   **f. Просмотреть статистику зоопарка:**
    *   **Реализация:**
        *   `GET /api/statistics`: Получение статистики (общее кол-во животных/вольеров, свободные вольеры, распределение по типам вольеров).
    *   **Классы/Модули:**
        *   Контроллер: `ZooManagement.WebAPI/Controllers/StatisticsController.cs` (метод `GetStatistics`).
        *   Сервис Приложения: `ZooManagement.Application/Services/ZooStatisticsService.cs` (метод `GetStatisticsAsync`).

**Дополнительно реализованные операции:**

*   Получение информации о животных/вольерах по ID и списком (`GET` эндпоинты в `AnimalsController` и `EnclosuresController`).
*   Изменение расписания кормления (`PUT /api/feedingschedules/{id}` в `FeedingSchedulesController`).
*   Отметка расписания кормления как выполненного (`POST /api/feedingschedules/{id}/complete` в `FeedingSchedulesController`).
*   Удаление расписания кормления (`DELETE /api/feedingschedules/{id}` в `FeedingSchedulesController`).

### 2. Примененные Концепции и Принципы (Требование 6b)

**2.1. Принципы Clean Architecture (Требование 2)**

*   **Структура проекта:** Проект разделен на четыре основных слоя согласно Clean Architecture:
    *   `ZooManagement.Domain`: Ядро системы, содержит сущности, VO, интерфейсы репозиториев.
    *   `ZooManagement.Application`: Содержит логику приложения (сервисы) и DTO.
    *   `ZooManagement.Infrastructure`: Реализует интерфейсы (репозитории, диспетчер событий) и содержит детали инфраструктуры (in-memory хранилище).
    *   `ZooManagement.WebAPI`: Слой представления (REST API контроллеры), точка входа для пользователя.
*   **Правило зависимостей (2a):** Зависимости направлены строго внутрь: `WebAPI` -> `Application` <- `Infrastructure`, `Application` -> `Domain`. Проект `ZooManagement.Domain` не имеет зависимостей от других проектов решения. Это проверено через анализ файлов `.csproj`.
*   **Зависимости через интерфейсы (2b):** Взаимодействие между слоями осуществляется через абстракции (интерфейсы):
    *   `Application` зависит от `Domain` через интерфейсы репозиториев (`IAnimalRepository`, `IEnclosureRepository`, `IFeedingScheduleRepository`), определенных в `ZooManagement.Domain/Interfaces/`.
    *   `Infrastructure` реализует эти интерфейсы (`InMemoryAnimalRepository`, и т.д.) в `ZooManagement.Infrastructure/Persistence/`.
    *   `WebAPI` зависит от `Application` через интерфейсы сервисов (`IAnimalService`, `IAnimalTransferService`, `IFeedingOrganizationService`, `IZooStatisticsService`), определенных в `ZooManagement.Application/Services/`.
    *   Для демонстрации также определен интерфейс `IEventDispatcher` в `ZooManagement.Application/Interfaces/` и реализован в `ZooManagement.Infrastructure/EventDispatching/`.
*   **Изоляция бизнес-логики (2c):**
    *   **Доменная логика** (правила предметной области, инварианты) инкапсулирована исключительно в слое `ZooManagement.Domain`. Примеры: проверка совместимости и вместимости в `Enclosure.AddAnimal`, валидация данных в конструкторах Value Objects (`AnimalName`, `Species`), смена статуса в `Animal.Heal`.
    *   **Логика приложения** (оркестрация Use Cases, координация репозиториев, маппинг в DTO) находится только в слое `ZooManagement.Application`, в классах сервисов (`AnimalTransferService`, `ZooStatisticsService` и др.).
    *   Слои `ZooManagement.Infrastructure` (детали хранения) и `ZooManagement.WebAPI` (обработка HTTP, маршрутизация) не содержат бизнес-логики.

**2.2. Концепции Domain-Driven Design (DDD) (Требование 3)**

*   **Сущности (Entities):** Определены основные сущности предметной области с идентификаторами и жизненным циклом: `Animal`, `Enclosure`, `FeedingSchedule`. Находятся в `ZooManagement.Domain/Entities/`. Они инкапсулируют состояние и связанное с ним поведение (методы `AddAnimal`, `Feed`, `MarkAsDone` и т.д.).
*   **Объекты-значения (Value Objects) (3a):** Использованы для представления понятий, не имеющих собственной идентичности, и для инкапсуляции примитивных типов с правилами валидации. Реализованы как неизменяемые типы (`record` или `enum`) в `ZooManagement.Domain/ValueObjects/`:
    *   `AnimalName`, `Species`, `FoodType` (как `record` с проверками).
    *   `EnclosureType`, `AnimalStatus`, `Sex` (как `enum`).
*   **Репозитории (Repositories):** Определены интерфейсы репозиториев (`IAnimalRepository`, `IEnclosureRepository`, `IFeedingScheduleRepository`) в `ZooManagement.Domain/Interfaces/` как абстракция для доступа к данным. Реализации (`InMemory...Repository`) находятся в `ZooManagement.Infrastructure/Persistence/`.
*   **Доменные Сервисы (Domain Services):** Хотя явных доменных сервисов в слое `Domain` нет, логика, координирующая взаимодействие нескольких сущностей (которая могла бы быть в доменном сервисе), реализована в сервисах приложения, таких как `AnimalTransferService` в `ZooManagement.Application/Services/`.
*   **Доменные События (Domain Events):** Определены классы `AnimalMovedEvent` и `FeedingTimeEvent` в `ZooManagement.Domain/Events/` для фиксации значимых событий в домене. Предусмотрена возможность их публикации через `IEventDispatcher`.
*   **Инкапсуляция бизнес-правил (3b):** Бизнес-правила и инварианты реализованы **внутри методов доменных сущностей и конструкторов Value Objects**, а не в сервисах приложения. Это обеспечивает высокую связность (cohesion) и предотвращает анемичную модель домена. Примеры:
    *   **`Enclosure.AddAnimal(Animal animal)`:** Проверяет, не переполнен ли вольер (`IsFull`), и совместим ли тип вольера с видом животного (`CanAccommodate(animal.Species)`), выбрасывая `EnclosureFullException` или `IncompatibleAnimalTypeException` при нарушении.
    *   **`Animal.Create(...)`:** Проверяет корректность даты рождения (не может быть в будущем).
    *   **`Animal.AssignToEnclosure(Guid enclosureId)`:** Может проверять статус животного (например, запрещать перемещение больных животных, если это правило).
    *   **`Animal.Heal()`:** Проверяет текущий статус животного (`Status == AnimalStatus.Sick`) перед изменением на `Healthy`.
    *   **`FeedingSchedule.Create(...)` / `ChangeSchedule(...)`:** Проверяют, что время кормления (`FeedingTime`) установлено в будущем.
    *   **`FeedingSchedule.MarkAsDone()` / `ChangeSchedule(...)`:** Проверяют, не было ли расписание уже выполнено (`IsCompleted`).
    *   **Конструкторы Value Objects (`AnimalName`, `Species`, `FoodType`):** Проверяют входные строки на `null` или пустое значение.

### 3. Хранение Данных и Тестирование

*   **Хранение данных:** Реализовано в слое `Infrastructure` с использованием in-memory хранилища на базе потокобезопасных словарей `ConcurrentDictionary<Guid, T>` в классах `InMemoryAnimalRepository`, `InMemoryEnclosureRepository`, `InMemoryFeedingScheduleRepository`.
*   **Тестирование API:** Приложение сконфигурировано для использования Swagger UI (`Program.cs` в `ZooManagement.WebAPI`), что позволяет интерактивно тестировать все реализованные API эндпоинты (добавление, получение, удаление, перемещение и т.д.) через браузер при запуске в среде `Development`. Файл `Properties/launchSettings.json` настроен для удобства запуска.